import datetime
from flask import Blueprint, request, Response, make_response, jsonify
from database_manager import DB_Manager
import jwt
from config_holder import conf

login_page = Blueprint('login_page', __name__)


@login_page.route('/login', methods=['POST'])
def login():
    data = request.get_json()

    if 'username' in data and 'password' in data:
        result = DB_Manager.check_credentials(data['username'], data['password'])
        user = DB_Manager.get_user(data['username'])

        if result == "OK":
            if 'stay_logged_in' in data and data['stay_logged_in']:
                token = jwt.encode({'id': user.id},
                                   conf['secret_key'])
            else:
                token = jwt.encode({'id': user.id,
                                    'exp': datetime.datetime.utcnow() + datetime.timedelta(seconds=15)},
                                   conf['secret_key'])

            return make_response(jsonify({'token': token.decode('UTF-8')}), 201)

        return Response(result, 400)
    else:
        return Response("Missing arguments", 400)
