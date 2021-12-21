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
        public string Message { get; set; }

        public ContactItem(string nickname,int nicknameID, string message)
        {
            Nickname = $"{nickname}#{nicknameID}";
            Message = message;
        }
    }
}
