import jwt
from flask import Blueprint, request, Response
from config_holder import conf
from token_handler import token_required

socket_page = Blueprint('socket_page', __name__)


@socket_page.route('/request_socket', methods=['POST'])
@token_required
def request_socket():
    return Response("OK", 101)
