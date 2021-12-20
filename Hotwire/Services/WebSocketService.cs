using Hotwire.Model;
using Hotwire.View;
using Hotwire.ViewModel;
using SocketIOClient;
using System;
using System.Collections.Generic;
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

        public WebSocketService()
        {
            client.OnConnected += async (sender, e) =>
            {
                await client.EmitAsync("authorize_ticket", socketTicket);
            };

            client.On("ticket_accepted", response => 
            {
                //Connected = true;
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
    }
}
