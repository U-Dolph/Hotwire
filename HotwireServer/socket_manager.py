import json
from datetime import datetime

from flask import request
from app import app
from flask_socketio import SocketIO
from endpoints.setup_socket import ticket_cache
from database_manager import DB_Manager


app_socket = SocketIO(app)

sessions = {}


@app_socket.on('connect')
def connecting():
    print(request.sid + " trying to connect")


@app_socket.on('disconnect')
def disconnecting():
    print(request.sid + " disconnected")
    if request.sid in sessions.keys():
        sessions.pop(str(request.sid))


@app_socket.on('give_ticket')
def handle_ticket(ticket):
    print(request.sid + " sent a ticket")
    ticket = json.loads(ticket)

    if ticket in ticket_cache:
        if datetime.now() < datetime.strptime(ticket['expire_in'], '%Y-%m-%d %H:%M:%S'):
            print("valid ticket")
            sessions[str(request.sid)] = ticket['user_id']
            print(sessions)
        else:
            print("ticket expired")
            app_socket.emit('close_connection', to=request.sid)

        ticket_cache.pop(ticket_cache.index(ticket))
    else:
        print("invalid ticket")
        app_socket.emit('close_connection', to=request.sid)


@app_socket.on('query_users')
def query_users(nickname):
    results = DB_Manager.get_users_by_nickname(nickname)
    app_socket.emit('user_search_result', str(json.dumps([e.serialize() for e in results])), room=request.sid)


@app_socket.on('add_friend_request')
def add_friend(friend_id):
    sender_id = sessions[str(request.sid)]

    result = DB_Manager.add_friend(sender_id, friend_id)

    app_socket.emit('add_friend_completed', json.dumps(result), to=request.sid)