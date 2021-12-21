import hashlib
import datetime
import MySQLdb
from flask_mysqldb import MySQL

from models.Message import Message
from models.User import User


class DatabaseManager:
    def __init__(self):
        self.mysql = None

    def register_app(self, flask_app):
        with flask_app.app_context():
            self.mysql = MySQL(flask_app)

    def user_exists(self, username):
        try:
            _cursor = self.mysql.connection.cursor()
            _cursor.execute("SELECT ID FROM users WHERE Username=%s", (username, ))
            result = _cursor.fetchall()
            _cursor.close()

            if len(result) > 0:
                return True

            return False
        except MySQLdb.Error as e:
            print(e)
            return True

    def get_password(self, username):
        _cursor = self.mysql.connection.cursor()
        _cursor.execute("SELECT PWD FROM users WHERE USERNAME=%s", (username,))
        result = _cursor.fetchall()
        _cursor.close()

        return result[0][0]

    def check_nickname(self, nickname):
        _cursor = self.mysql.connection.cursor()
        _cursor.execute("SELECT NICKNAME_ID FROM users WHERE NICKNAME=%s order by NICKNAME_ID LIMIT 1", (nickname, ))

        result = _cursor.fetchone()

        if result:
            return result[0] + 1

        return 0

    def register_user(self, user):
        if not self.user_exists(user.username):
            try:
                _cursor = self.mysql.connection.cursor()

                user.nickname_id = self.check_nickname(user.nickname)

                _cursor.execute("INSERT INTO users(USERNAME, NICKNAME, NICKNAME_ID , PWD, STATUS, TIME_REGISTERED) "
                                "VALUES (%s, %s, %s, %s, %s, %s)",
                                (user.username, user.nickname, user.nickname_id,
                                 hashlib.sha256(user.password.encode("ascii")).hexdigest(),
                                 user.status, user.time_registered, ))

                self.mysql.connection.commit()

                _cursor.close()
                return "Registration successful, now you can log in"
            except MySQLdb.Error as e:
                print(e)
                return "Some error occured on our server :("

        return "Username already taken!"

    def check_credentials(self, username, password):
        if self.user_exists(username):
            self.get_user_by_username(username)
            if self.get_password(username) == hashlib.sha256(password.encode("ascii")).hexdigest():
                return "OK"

            return "Invalid password"
        else:
            return "Invalid username"

    def get_user_by_id(self, id):
        _cursor = self.mysql.connection.cursor()
        _cursor.execute("SELECT * FROM users WHERE ID=%s", (id,))
        result = _cursor.fetchall()
        _cursor.close()

        result = result[0]

        return User({
            "ID": result[0], "Username": result[1], "Nickname": result[2],
            "NicknameID": result[3], "Password": result[4], "Status": result[5],
            "TimeRegistered": result[6]
        })

    def get_user_by_username(self, username):
        _cursor = self.mysql.connection.cursor()
        _cursor.execute("SELECT * FROM users WHERE USERNAME=%s", (username,))
        result = _cursor.fetchall()
        _cursor.close()

        result = result[0]

        return User({
            "ID": result[0], "Username": result[1], "Nickname": result[2],
            "NicknameID": result[3], "Password": result[4], "Status": result[5],
            "TimeRegistered": result[6]
        })

    def get_user_by_nickname(self, nickname, nickname_id):
        _cursor = self.mysql.connection.cursor()
        _cursor.execute("SELECT * FROM users WHERE NICKNAME = %s AND NICKNAME_ID = %s", (nickname, nickname_id))
        result = _cursor.fetchall()
        _cursor.close()

        if len(result) > 0:
            result = result[0]

            return User({
                         "ID": result[0], "Username": result[1], "Nickname": result[2],
                         "NicknameID": result[3], "Status": result[5]
                     })

        return None

    def add_friend(self, sender_id, receiver_nickname, receiver_nickname_id):
        sender = self.get_user_by_id(sender_id)
        receiver = self.get_user_by_nickname(receiver_nickname, receiver_nickname_id)

        if receiver is not None:
            if self.user_exists(sender.username) and self.user_exists(receiver.username):
                if receiver.id == sender.id:
                    return "You can't add yourself"

                _cursor = self.mysql.connection.cursor()

                try:
                    _cursor.execute("INSERT INTO friendships(UserID1, UserID2, Pending, Time_Since) "
                                    "VALUES (%s, %s, %s, %s)",
                                    (int(sender.id), int(receiver.id), 0,
                                     datetime.datetime.now().strftime('%Y-%m-%d %H:%M:%S'),))

                    self.mysql.connection.commit()
                except Exception as ex:
                    print(ex)

                    if "Duplicate entry" in ex.args[1]:
                        return "You are already friends"

                    return "Something went wrong :("

                _cursor.close()

                return "You are now friends"
            else:
                return "User not found"

        return "User not found"

    def get_friends_by_id(self, user_id):
        print("requesting users")
        _cursor = self.mysql.connection.cursor()

        query = "SELECT * FROM users u " \
                "LEFT JOIN friendships f ON f.UserID2=u.ID WHERE f.UserID1 = %s " \
                "UNION " \
                "SELECT * FROM users u " \
                "LEFT JOIN friendships f ON f.UserID1=u.ID WHERE f.UserID2 = %s "

        _cursor.execute(query, (user_id, user_id))

        result = _cursor.fetchall()

        users = []

        for row in result:
            last_message = self.get_last_message_with_given_user(user_id, row[0])
            print(row[0])
            users.append(
                User({
                    "ID": row[0], "Username": row[1], "Nickname": row[2],
                    "NicknameID": row[3], "Status": row[5], "LastMessage": last_message.content
                })
            )

        return users

    def get_all_messages_by_id(self, user_id):
        _cursor = self.mysql.connection.cursor()

        friends = self.get_friends_by_id(user_id)

        result = {}

        for friend in friends:
            messages_with_friend = self.get_messages_with_given_user(user_id, friend.id)
            result[f"{friend.nickname}#{friend.nickname_id}"] = [e.serialize() for e in messages_with_friend]

        return result

    def get_messages_with_given_user(self, user_id, friend_id):
        _cursor = self.mysql.connection.cursor()

        query = "SELECT * FROM messages m " \
                "LEFT JOIN users u ON m.SenderID=u.ID WHERE m.ReceiverID = %s AND m.SenderID = %s " \
                "UNION " \
                "SELECT * FROM messages m " \
                "LEFT JOIN users u ON m.ReceiverID=u.ID WHERE m.SenderID = %s AND m.ReceiverID = %s " \
                "ORDER BY UNIX_TIMESTAMP(TimeSent)"

        _cursor.execute(query, (user_id, friend_id, user_id, friend_id))
        result = _cursor.fetchall()

        messages = []

        for row in result:
            sender = self.get_user_by_id(row[1])
            sender_nickname = f"{sender.nickname}#{sender.nickname_id}"
            messages.append(Message([row[0], row[1], row[2], sender_nickname, row[3]]))

        return messages

    def get_last_message_with_given_user(self, user_id, friend_id):
        _cursor = self.mysql.connection.cursor()

        query = "SELECT * FROM messages m " \
                "LEFT JOIN users u ON m.SenderID=u.ID WHERE m.ReceiverID = %s AND m.SenderID = %s " \
                "UNION " \
                "SELECT * FROM messages m " \
                "LEFT JOIN users u ON m.ReceiverID=u.ID WHERE m.SenderID = %s AND m.ReceiverID = %s " \
                "ORDER BY UNIX_TIMESTAMP(TimeSent) DESC " \
                "LIMIT 1"

        _cursor.execute(query, (user_id, friend_id, user_id, friend_id))
        result = _cursor.fetchall()

        if len(result) > 0:
            result = result[0]
            return Message([result[0], result[1], result[2], result[7], result[3]])
        else:
            return Message([0, user_id, friend_id, "No message", "You didn't speak yet"])

    def send_message_to_user(self, sender_id, receiver_id, content):
        _cursor = self.mysql.connection.cursor()

        try:
            _cursor.execute("INSERT INTO messages(SenderID, ReceiverID, Content, TimeSent) "
                            "VALUES (%s, %s, %s, %s)",
                            (int(sender_id), int(receiver_id), content,
                             datetime.datetime.now().strftime('%Y-%m-%d %H:%M:%S'), ))

            self.mysql.connection.commit()

            result = _cursor.execute("SELECT ID from messages ORDER BY ID DESC LIMIT 1")
            last_msg_id = _cursor.fetchone()[0]
            return last_msg_id
        except Exception as ex:
            print(ex)


DB_Manager = DatabaseManager()
