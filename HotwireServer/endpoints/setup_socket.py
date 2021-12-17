import datetime
import uuid
import jwt
from flask import Blueprint, request, make_response, jsonify
from config_holder import conf
from token_handler import token_required

socket_page = Blueprint('socket_page', __name__)

ticket_cache = []


@socket_page.route('/request_socket', methods=['POST'])
@token_required
def request_socket():
    data = jwt.decode(request.headers['x-access-token'], conf['secret_key'])
    token = {
        "expire_in": (datetime.datetime.now().replace(microsecond=0) + datetime.timedelta(seconds=5)).strftime('%Y-%m-%d %H:%M:%S'),
        "user_id": data['id'],
        "id": uuid.uuid4().hex,
        "sender_ip": request.remote_addr,
        "timestamp": datetime.datetime.now().strftime('%Y-%m-%d %H:%M:%S')
    }

    ticket_cache.append(token)
    return make_response(jsonify(token), 200)
