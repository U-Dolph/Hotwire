import datetime
import json
from flask import Blueprint, Response, request
from token_handler import token_required

socket_page = Blueprint('socket_page', __name__)

ticket_cache = []


@socket_page.route('/request_socket', methods=['POST'])
@token_required
def request_socket():
    token = json.dumps({
        "sender_ip": request.remote_addr,
        "timestamp": str(datetime.datetime.utcnow()),
        "expire_in": str(datetime.datetime.utcnow() + datetime.timedelta(seconds=10))})

    ticket_cache.append(token)
    return Response(json.dumps(token), 200)
