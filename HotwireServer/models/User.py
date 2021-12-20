import datetime


class User:
    def __init__(self, data):
        self.id = data['ID'] if 'ID' in data.keys() else None
        self.username = data['Username']
        self.nickname = data['Nickname']
        self.nickname_id = data['NicknameID'] if 'NicknameID' in data.keys() else 0
        self.password = data['Password'] if 'Password' in data.keys() else ""
        self.status = 0
        self.time_registered = datetime.datetime.utcnow().strftime('%Y-%m-%d %H:%M:%S')

    def serialize(self):
        return {
            'ID': int(self.id),
            'Username': self.username,
            'Nickname': self.nickname,
            'NicknameID': int(self.nickname_id),
            'Status': int(self.status)
        }
