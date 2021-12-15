import hashlib

import MySQLdb
from flask_mysqldb import MySQL


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

    def get_user(self, username):
        _cursor = self.mysql.connection.cursor()
        _cursor.execute("SELECT * FROM users WHERE USERNAME=%s", (username,))
        result = _cursor.fetchall()
        _cursor.close()

        print(result[0])

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

                _cursor.execute("INSERT INTO `users`(USERNAME, NICKNAME, NICKNAME_ID , PWD, STATUS, TIME_REGISTERED) "
                                "VALUES (%s, %s, %s, %s, %s, %s)",
                                (user.username, user.nickname, user.nickname_id,
                                 user.password, user.status, user.time_registered, ))

                self.mysql.connection.commit()

                _cursor.close()
                return "User registered"
            except MySQLdb.Error as e:
                print(e)
                return "ERROR"

        return "User already exists"

    def check_credentials(self, username, passwd):
        if self.user_exists(username):
            if self.get_password(username) == hashlib.sha256(passwd.encode("ascii")).hexdigest():
                return "OK"

            return "Invalid password"
        else:
            return "Invalid username"


DB_Manager = DatabaseManager()
