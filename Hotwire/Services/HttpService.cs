using Hotwire.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
    }
}
