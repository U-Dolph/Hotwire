from flask import Blueprint, request, Response
from database_manager import DB_Manager

register_page = Blueprint('register_page', __name__)


@register_page.route('/register', methods=['POST'])
def register_user():
    user = request.get_json()
    print(user)

    if "username" not in user or "nickname" not in user or "password" not in user:
        return Response("KO", 403)

    result = DB_Manager.register_user(user)

    return Response(result, 200)
