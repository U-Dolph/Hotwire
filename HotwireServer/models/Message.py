class Message:
    def __init__(self, data):
        self.id = data[0]
        self.sender_id = data[1]
        self.receiver_id = data[2]
        self.sender_nickname = data[3]
        self.content = data[4]

    def serialize(self):
        return {
            'ID': int(self.id),
            'SenderID': self.sender_id,
            'ReceiverID': self.receiver_id,
            'Nickname': self.sender_nickname,
            'Content': self.content,
        }

    def __str__(self):
        return f"ID: {self.id}, SenderID: {self.sender_id}, ReciverID: {self.receiver_id}, Nickname: {self.sender_nickname}, Content{self.content}"
