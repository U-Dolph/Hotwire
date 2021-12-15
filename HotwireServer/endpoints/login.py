import json
from flask import Blueprint, request, Response
from database_manager import DB_Manager
from models.User import User

login_page = Blueprint('login_page', __name__)


@login_page.route('/login', methods=['POST'])
def login():
    data = request.get_json()

    if 'username' in data and 'password' in data:
        result = DB_Manager.check_credentials(data['username'], data['password'])

        return Response(result, 200 if result == "OK" else 400)
    else:
        return Response("Missing arguments", 400)
