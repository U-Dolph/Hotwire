import datetime
import hashlib


class User:
    def __init__(self, data):
        self.username = data['username']
        self.nickname = data['nickname']
        self.nickname_id = 0
        self.password = hashlib.sha256(data['password'].encode("ascii")).hexdigest()
        self.status = 0
        self.time_registered = datetime.datetime.utcnow().strftime('%Y-%m-%d %H:%M:%S')
