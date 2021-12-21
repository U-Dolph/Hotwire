using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotwire.Model
{
    public class Message
    {
        public int ID { get; set; }
        public int SenderID { get; set; }
        public int ReciverID { get; set; }
        public string SenderNickname { get; set; }
        public string Content { get; set; }
    }
}
