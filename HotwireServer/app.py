from database_manager import DB_Manager
from endpoints import register, login, setup_socket
from flask import Flask
from config_holder import conf

app = Flask(__name__)
app.app_context()

app.config["MYSQL_HOST"] = conf['host']
app.config['MYSQL_USER'] = conf['user']
app.config['MYSQL_PASSWORD'] = conf['pw']
app.config['MYSQL_DB'] = conf['db']
app.config['SECRET_KEY'] = conf['secret_key']

app.register_blueprint(register.register_page)
app.register_blueprint(login.login_page)
app.register_blueprint(setup_socket.socket_page)

DB_Manager.register_app(app)
