using Hotwire.Model;
using Hotwire.View;
using Hotwire.ViewModel;
using Newtonsoft.Json;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hotwire.Services
{
    public class WebSocketService
    {
        //public bool Connected { get; private set; }
        private SocketIO client = new SocketIO(Constants.ServerUrl);
        private string socketTicket;
        public List<User> Friends = new List<User>();

        public WebSocketService()
        {
            client.Options.Reconnection = false;
            client.OnConnected += async (sender, e) =>
            {
                await client.EmitAsync("authorize_ticket", socketTicket);
            };

            client.On("ticket_accepted", response => 
            {
                //Connected = true;
            });

            client.On("friendlist_result", response =>
            {
                Friends = JsonConvert.DeserializeObject<List<User>>(response.GetValue<string>());
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
        }

        public async void GetFriends()
        {
            await client.EmitAsync("get_friends_request");
        }
    }
}
