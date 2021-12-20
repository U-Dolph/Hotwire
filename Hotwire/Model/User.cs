using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotwire.Model
{
    public class User
    {
        public string Username { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public bool StayLoggedIn { get; set; }

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
    }
}
