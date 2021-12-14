import json

from flask import Flask
from database_manager import DB_Manager
from endpoints.register import register_page


if __name__ == '__main__':
    app = Flask(__name__)
    app.app_context()

    with open("credentials.conf") as f:
        data = json.load(f)

        app.config["MYSQL_HOST"] = data['host']
        app.config['MYSQL_USER'] = data['user']
        app.config['MYSQL_PASSWORD'] = data['pw']
        app.config['MYSQL_DB'] = data['db']

    DB_Manager.register_app(app)

    app.register_blueprint(register_page)

    app.run(host="127.0.0.1", port="5000", debug=True)
