using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotwire.Model
{
    public class ContactItem
    {
        public string Username { get; set; }
        public string LastMessage { get; set; }

        public ContactItem(string username, string lastMessage)
        {
            Username = username;
            LastMessage = lastMessage;
        }
    }
}
