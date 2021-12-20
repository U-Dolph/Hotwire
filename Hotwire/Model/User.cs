using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotwire.Model
{
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Nickname { get; set; }
        public int NicknameID { get; set; }
        public string Password { get; set; }
        public bool StayLoggedIn { get; set; }
        public int Status { get; set; }

        //For registering
        public User(string username, string nickname, string password)
        {
            Username = username;
            Nickname = nickname;
            Password = password;
        }

        //For logging in
        public User(string username, string password, bool stayLoggedIn)
        {
            Username = username;
            Password = password;
            StayLoggedIn = stayLoggedIn;
        }

        [JsonConstructor]
        public User(int ID, string Username, string Nickname, int NicknameID, int Status)
        {
            this.ID = ID;
            this.Username = Username;
            this.Nickname = Nickname;
            this.NicknameID = NicknameID;
            this.Status = Status;
        }
    }
}
