using Hotwire.Model;
using Newtonsoft.Json;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Hotwire.Services
{
    public class WebSocketService : INotifyPropertyChanged
    {
        private SocketIO client;
        public bool Connected { get; set; }
        private string socketTicket;
        public List<User> Friends { get; set; }
        public List<Message> CurrentMessages { get; set; }
        public string Response { get; set; }
        public User CurrentUser { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public WebSocketService()
        {
            client = new SocketIO(Constants.ServerUrl);
            Friends = new List<User>();

            client.Options.Reconnection = false;
            client.OnConnected += async (sender, e) =>
            {
                await client.EmitAsync("authorize_ticket", socketTicket);
            };

            client.On("ticket_accepted", async response => 
            {
                Connected = true;
                await client.EmitAsync("get_identity_request");
            });

            client.On("friendlist_result", response =>
            {
                Friends = JsonConvert.DeserializeObject<List<User>>(response.GetValue<string>());
            });

            client.On("add_friend_completed", async response =>
            {
                await client.EmitAsync("get_friends_request");
                Response = response.GetValue<string>();
            });

            client.On("identity_result", response =>
            {
                CurrentUser = JsonConvert.DeserializeObject<User>(response.GetValue<string>());
            });

            client.On("message_with_user_result", response =>
            {
                CurrentMessages = JsonConvert.DeserializeObject<List<Message>>(response.GetValue<string>());
            });

            client.On("new_message", async response =>
            {
                await client.EmitAsync("get_messages_with_given_user_request", response.GetValue<int>());
            });
        }

        public async void ConnectToServer(string ticket)
        {
            socketTicket = ticket;

            await client.ConnectAsync();
        }

        public async void DisconnectFromServer()
        {
            await client.DisconnectAsync();
            Connected = false;
        }

        public async void GetFriends()
        {
            await client.EmitAsync("get_friends_request");
        }

        public async void AddFriend(string nickname, string nicknameID)
        {
            await client.EmitAsync("add_friend_request", new object[] {nickname, nicknameID});
        }

        public async void SendMessage(int userID, string content)
        {
            await client.EmitAsync("send_message_to_user", new object[] { userID, content });
        }

        public async void GetMessageWithUser(int userID)
        {
            await client.EmitAsync("get_messages_with_given_user_request", userID);
        }
    }
}
