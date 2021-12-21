using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Hotwire.Model
{
    public class ContactItem : INotifyPropertyChanged
    {
        private string _lastMessage;

        public event PropertyChangedEventHandler PropertyChanged;

        public int ID { get; set; }
        public string Nickname { get; set; }
        public int MessageID { get; set; }
        public string LastMessage 
        { 
            get => _lastMessage;
            set
            {
                _lastMessage = value;
                NotifyPropertyChanged();
            }
        }

        public ContactItem(int id, string nickname,int nicknameID, string message, int messageID)
        {
            ID = id;
            Nickname = $"{nickname}#{nicknameID}";
            LastMessage = message;
            MessageID = messageID;
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
