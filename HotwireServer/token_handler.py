from functools import wraps
import jwt
from flask import request, jsonify
from config_holder import conf

tokens_issued = []


def token_required(f):
    @wraps(f)
    def decorated(*args, **kwargs):
        token = None

        if 'x-access-token' in request.headers:
            token = request.headers['x-access-token']

        if not token:
            return jsonify({'message': 'Token is missing !!'}), 401

        try:

            data = jwt.decode(token, conf['secret_key'])
        except Exception as e:
            return jsonify({
                'message': 'Token is invalid !!'
            }), 401
        # returns the current logged in users contex to the routes
        return f(*args, **kwargs)

    return decorated
