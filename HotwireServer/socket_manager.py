import json
from datetime import datetime

from flask import request
from app import app
from flask_socketio import SocketIO
from endpoints.setup_socket import ticket_cache
from database_manager import DB_Manager
from models.Message import Message

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


@app_socket.on('authorize_ticket')
def handle_ticket(ticket):
    print(request.sid + " sent a ticket")
    ticket = json.loads(ticket)

    if ticket in ticket_cache:
        if datetime.now() < datetime.strptime(ticket['expire_in'], '%Y-%m-%d %H:%M:%S'):
            print("valid ticket")
            sessions[str(request.sid)] = ticket['user_id']
            print(sessions)
            app_socket.emit('ticket_accepted', to=request.sid)
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
def add_friend(friend_nickname, friend_nickname_id):
    sender_id = sessions[str(request.sid)]
    receiver = DB_Manager.get_user_by_nickname(friend_nickname, friend_nickname_id)

    if receiver is not None and receiver.id in sessions.values():
        receiver_session = list(sessions.keys())[list(sessions.values()).index(receiver.id)]
        app_socket.emit('new_friend', sender_id, to=receiver_session)

    result = DB_Manager.add_friend(sender_id, friend_nickname, friend_nickname_id)

    app_socket.emit('add_friend_completed', result, to=request.sid)


@app_socket.on('get_friends_request')
def get_friends():
    sender_id = sessions[str(request.sid)]
    results = DB_Manager.get_friends_by_id(sender_id)
    app_socket.emit('friendlist_result', str(json.dumps([e.serialize() for e in results])), room=request.sid)


@app_socket.on('get_identity_request')
def get_identity():
    sender_id = sessions[str(request.sid)]
    result = DB_Manager.get_user_by_id(sender_id)
    app_socket.emit('identity_result', json.dumps(result.serialize()), room=request.sid)


@app_socket.on('get_all_messages_request')
def get_all_messages():
    sender_id = sessions[str(request.sid)]
    results = DB_Manager.get_all_messages_by_id(sender_id)

    print(results)
    app_socket.emit('message_list_result', json.dumps(results), room=request.sid)


@app_socket.on('get_messages_with_given_user_request')
def get_messages_with_user(friend_id):
    print("requesting messages!")
    sender_id = sessions[str(request.sid)]
    receiver_user = DB_Manager.get_user_by_id(friend_id)
    results = DB_Manager.get_messages_with_given_user(sender_id, friend_id)

    app_socket.emit('message_with_user_result', json.dumps({
        f"{receiver_user.nickname}#{receiver_user.nickname_id}":
            [e.serialize() for e in results]}),
                    room=request.sid)


@app_socket.on('send_message_to_user')
def send_message(receiver_id, content):
    sender_id = sessions[str(request.sid)]
    sender_user = DB_Manager.get_user_by_id(sender_id)
    receiver_user = DB_Manager.get_user_by_id(receiver_id)
    last_message_id = DB_Manager.send_message_to_user(sender_id, receiver_id, content)
    print(f"{sender_id} to: {receiver_id} => {content}")
    
    if receiver_id in sessions.values():
        receiver_session = list(sessions.keys())[list(sessions.values()).index(receiver_id)]

        msg = Message([last_message_id, sender_id, receiver_id,
                       f"{sender_user.nickname}#{sender_user.nickname_id}", content])

        app_socket.emit('new_message', json.dumps(msg.serialize()), to=receiver_session)

    results = DB_Manager.get_messages_with_given_user(sender_id, receiver_id)
    app_socket.emit('message_with_user_result', json.dumps({
        f"{receiver_user.nickname}#{receiver_user.nickname_id}":
            [e.serialize() for e in results]}),
                    room=request.sid)
