class Message:
    def __init__(self, data):
        self.id = data[0]
        self.sender_id = data[1]
        self.receiver_id = data[2]
        self.sender_nickname = data[3]
        # self.receiver_nickname = data[4]
        self.content = data[4]

    def serialize(self):
        return {
            'ID': int(self.id),
            'SenderID': self.sender_id,
            'ReceiverID': self.receiver_id,
            'SenderNickname': self.sender_nickname,
            # 'ReceiverNickname': self.receiver_nickname,
            'Content': self.content,
        }
