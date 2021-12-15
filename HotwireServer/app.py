import json
from database_manager import DB_Manager
from endpoints import register, login
from flask import Flask
from flask_socketio import SocketIO


if __name__ == '__main__':
    app = Flask(__name__)
    app.app_context()

    with open("credentials.conf") as f:
        data = json.load(f)

        app.config["MYSQL_HOST"] = data['host']
        app.config['MYSQL_USER'] = data['user']
        app.config['MYSQL_PASSWORD'] = data['pw']
        app.config['MYSQL_DB'] = data['db']
        app.config['SECRET_KEY'] = data['secret_key']

    DB_Manager.register_app(app)

    app.register_blueprint(register.register_page)
    app.register_blueprint(login.login_page)

    app_socket = SocketIO(app)

    # app.run(host="127.0.0.1", port="5000", debug=True)
    app_socket.run(app, host='0.0.0.0', port=5000, debug=True)
