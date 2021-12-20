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
                try
                {
                    Token token = JsonConvert.DeserializeObject<Token>(msg.Content.ReadAsStringAsync().Result);
                }
                catch (Exception)
                {

                }

                //if (msg.IsSuccessStatusCode)
                //    request_socket(token);

                return msg.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //private async void request_socket(Token token)
        //{
        //    client.DefaultRequestHeaders.Clear();
        //    client.DefaultRequestHeaders.Add("x-access-token", token.Body);

        //    try
        //    {
        //        HttpResponseMessage msg = await client.PostAsync("http://dolphvault.duckdns.org:1234/request_socket", new StringContent(String.Empty));

        //        if (msg.StatusCode == System.Net.HttpStatusCode.OK)
        //        {
        //            try
        //            {
        //                .Instance.SocketTicket = msg.Content.ReadAsStringAsync().Result;
        //                await SocketHandler.Instance.SocketClient.ConnectAsync();
        //            }
        //            catch (Exception ex) { MessageBox.Show(ex.Message); }
        //        }

        //        Invoke((MethodInvoker)(() => socketLabel.Text = $"{msg.StatusCode}; {msg.Content.ReadAsStringAsync().Result}"));
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "Error");
        //    }

        //}
    }
}
