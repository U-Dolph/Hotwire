from flask import Blueprint, request, Response, make_response, jsonify
from database_manager import DB_Manager
from models.User import User

register_page = Blueprint('register_page', __name__)


@register_page.route('/register', methods=['POST'])
def register_user():
    data = request.get_json()

    if "username" not in data or "nickname" not in data or "password" not in data:
        return make_response(jsonify({"reason": "Invalid arguments"}), 403)

    result = DB_Manager.register_user(User(data))

    return make_response(jsonify({"reason": result}), 200 if result == "User registered" else 400)
