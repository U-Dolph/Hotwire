using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotwire.Model
{
    public class ContactItem
    {
        public string Nickname { get; set; }
        public string LastMessage { get; set; }

        public ContactItem(string nickname,int nicknameID, string lastMessage)
        {
            Nickname = $"{nickname}#{nicknameID}";
            LastMessage = lastMessage;
        }
    }
}
