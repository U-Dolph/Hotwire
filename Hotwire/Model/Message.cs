using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hotwire.Model
{
    public class Message
    {
        public int ID { get; set; }
        public int SenderID { get; set; }
        public int ReciverID { get; set; }
        public string Nickname { get; set; }
        public string Content { get; set; }
        public HorizontalAlignment Aligment{ get; set; } 
    }
}
