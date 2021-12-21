using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotwire.Model
{
    public class ContactItem
    {
        public int ID { get; set; }
        public string Nickname { get; set; }
        public string Message { get; set; }

        public ContactItem(int id, string nickname,int nicknameID, string message)
        {
            ID = id;
            Nickname = $"{nickname}#{nicknameID}";
            Message = message;
        }
    }
}
