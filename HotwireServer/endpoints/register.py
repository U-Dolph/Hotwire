from flask import Blueprint, request, Response, make_response, jsonify
from database_manager import DB_Manager
from models.User import User

register_page = Blueprint('register_page', __name__)


@register_page.route('/register', methods=['POST'])
def register_user():
    data = request.get_json()
    print(data)

    if "Username" not in data or "Nickname" not in data or "Password" not in data:
        return make_response("Invalid parameters!", 403)

    result = DB_Manager.register_user(User(data))
    print(result)

    return make_response(result, 200)
