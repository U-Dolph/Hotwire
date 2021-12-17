import datetime
from flask import Blueprint, request, make_response, jsonify
from database_manager import DB_Manager
import jwt
from config_holder import conf

login_page = Blueprint('login_page', __name__)


@login_page.route('/login', methods=['POST'])
def login():
    data = request.get_json()

    if 'username' in data and 'password' in data:
        result = DB_Manager.check_credentials(data['username'], data['password'])

        if result == "OK":
            user = DB_Manager.get_user_by_username(data['username'])

            if 'stay_logged_in' in data and data['stay_logged_in']:
                token = jwt.encode({'id': user.id},
                                   conf['secret_key'])
            else:
                token = jwt.encode({'id': user.id,
                                    'exp': datetime.datetime.utcnow() + datetime.timedelta(seconds=15)},
                                   conf['secret_key'])

            return make_response(jsonify({'token': token.decode('UTF-8')}), 201)

        return make_response(jsonify({'reason': result}), 400)
    else:
        return make_response(jsonify({'reason': "Missing arguments"}), 400)
