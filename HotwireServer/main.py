from socket_manager import app_socket
from app import app

if __name__ == '__main__':
    app_socket.run(app, host='0.0.0.0', port=1234, debug=True)
