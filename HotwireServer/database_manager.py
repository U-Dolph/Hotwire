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
            _cursor.execute("SELECT `ID` FROM `users` WHERE Username=%s", (username, ))
            result = _cursor.fetchall()
            _cursor.close()

            if len(result) > 0:
                return True

            return False
        except MySQLdb.Error as e:
            print(e)
            return True

    def register_user(self, user):
        if not self.user_exists(user.username):
            try:
                _cursor = self.mysql.connection.cursor()
                _cursor.execute("INSERT INTO `users`(USERNAME, NICKNAME, PWD, STATUS) "
                                "VALUES (%s, %s, %s, %s)",
                                (user.username, user.nickname, user.password, user.status, ))

                self.mysql.connection.commit()

                _cursor.close()
                return "User registered"
            except MySQLdb.Error as e:
                print(e)
                return "ERROR"

        return "User already exists"

    def remove_user(self, user_id):
        print("removing user")

    def check_credentials(self, username, passwd):
        print(f"checking credentials for {username} : {passwd}")


DB_Manager = DatabaseManager()