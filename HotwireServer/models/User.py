import datetime
import hashlib


class User:
    def __init__(self, data):
        self.id = data['id'] if 'id' in data.keys() else None
        self.username = data['username']
        self.nickname = data['nickname']
        self.nickname_id = 0
        self.password = data['password']
        self.status = 0
        self.time_registered = datetime.datetime.utcnow().strftime('%Y-%m-%d %H:%M:%S')

    def serialize(self):
        return {
            'ID': int(self.id),
            'Username': self.username,
            'Nickname': self.nickname,
            'NicknameID': self.nickname_id,
            'Status': int(self.status)
        }
