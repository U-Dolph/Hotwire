from app import app
from flask_socketio import SocketIO
from endpoints.setup_socket import ticket_cache


app_socket = SocketIO(app)
