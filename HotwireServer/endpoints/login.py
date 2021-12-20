import datetime
from flask import Blueprint, request, make_response, jsonify
from database_manager import DB_Manager
import jwt
from config_holder import conf

login_page = Blueprint('login_page', __name__)


@login_page.route('/login', methods=['POST'])
def login():
    data = request.get_json()
    print(data)

    if 'Username' in data and 'Password' in data:
        result = DB_Manager.check_credentials(data['Username'], data['Password'])

        if result == "OK":
            user = DB_Manager.get_user_by_username(data['Username'])

            if 'StayLoggedIn' in data and data['StayLoggedIn']:
                token = jwt.encode({'id': user.id},
                                   conf['secret_key'])
            else:
                token = jwt.encode({'id': user.id,
                                    'exp': datetime.datetime.utcnow() + datetime.timedelta(seconds=15)},
                                   conf['secret_key'])

            return make_response(jsonify({'Body': token.decode('UTF-8')}), 201)

        return make_response(result, 400)
    else:
        return make_response("Missing Arguments!", 400)
