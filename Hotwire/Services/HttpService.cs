using Hotwire.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hotwire.Services
{
    public class HttpService
    {
        private HttpClient client;

        public HttpService()
        {
            client = new HttpClient();
        }
        
        public async Task<string> RegisterUser(User user)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage msg = await client.PostAsync(Constants.RegisterUrl, content);

                return msg.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> LoginUser(User user)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage msg = await client.PostAsync(Constants.LoginUrl, content);

                if (msg.IsSuccessStatusCode)
                {
                    try
                    {
                        Token token = JsonConvert.DeserializeObject<Token>(msg.Content.ReadAsStringAsync().Result);

                        requestSocket(token);

                        return "Login successful";
                    }
                    catch (Exception)
                    {
                        return "Something went wrong :(";
                    }
                }

                return msg.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private async void requestSocket(Token token)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("x-access-token", token.Body);

            try
            {
                HttpResponseMessage msg = await client.PostAsync(Constants.SocketUrl, new StringContent(String.Empty));

                if (msg.IsSuccessStatusCode)
                {
                    try
                    {
                        string socketTicket = msg.Content.ReadAsStringAsync().Result;
                        App.WebSocketService.ConnectToServer(socketTicket);
                        
                    }
                    catch (Exception ex)
                    { }
                }
            }
            catch (Exception ex)
            { }
        }
    }
}
