using Hotwire.Model;
using Hotwire.ViewModel.Base;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Hotwire.ViewModel
{
    public class ChatViewModel : BaseViewModel
    {
        public ObservableCollection<ContactItem> Contacts { get; set; }
        public ObservableCollection<Message> Messages { get; set; }

        public RelayCommand DisconnectCommand { get; }
        public RelayCommand AddFriendCommand { get; }
        public RelayCommand SendMessageCommand { get; }
        private readonly BaseViewModel viewModel;

        public string AddFriendInput { get; set; }
        public string LabelMessage { get; set; }
        public string Nickname { get; set; }
        public int SelectedFriendIndex { get; set; }
        public string MessageContent { get; set; }

        public ChatViewModel(BaseViewModel viewModel)
        {
            this.viewModel = viewModel;
            DisconnectCommand = new RelayCommand(disconnect);
            AddFriendCommand = new RelayCommand(addFriend);
            SendMessageCommand = new RelayCommand(sendMessage);

            Contacts = new ObservableCollection<ContactItem>();
            Messages = new ObservableCollection<Message>();

            Nickname = "";

            App.WebSocketService.GetFriends();

            LabelMessage = " ";

            App.WebSocketService.PropertyChanged += propertiesChanged;

            PropertyChanged += selfPropertyChanged;
        }

        private void selfPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelctedFriendIndex")
            {
                App.WebSocketService.GetMessageWithUser(SelectedFriendIndex);
            }
        }

        private void disconnect()
        {
            App.WebSocketService.DisconnectFromServer();
            viewModel.SelectedViewModel = new LoginViewModel(viewModel);
        }

        private void propertiesChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Friends")
            {
                App.Current.Dispatcher.Invoke(delegate
                {
                    Contacts.Clear();

                    foreach (var item in App.WebSocketService.Friends)
                        Contacts.Add(new ContactItem(item.ID, item.Nickname, item.NicknameID, item.LastMessage));
                });

                App.WebSocketService.GetMessageWithUser(Contacts[SelectedFriendIndex].ID);
            }
            else if (e.PropertyName == "Response")
            {
                LabelMessage = App.WebSocketService.Response;
            }
            else if (e.PropertyName == "CurrentUser")
            {
                Nickname = $"{App.WebSocketService.CurrentUser.Nickname}#{App.WebSocketService.CurrentUser.NicknameID}";
            }
            else if (e.PropertyName == "CurrentMessages")
            {
                App.Current.Dispatcher.Invoke(delegate
                {
                    Messages.Clear();

                    foreach (var item in App.WebSocketService.CurrentMessages)
                        Messages.Add(new Message
                        {
                            ID = item.ID,
                            SenderID = item.SenderID,
                            ReciverID = item.ReciverID,
                            Content = item.Content,
                            SenderNickname = item.SenderNickname
                        });
                });

                Console.WriteLine();
            }
        }

        private void addFriend()
        {
            LabelMessage = " ";

            if (AddFriendInput != null && AddFriendInput.Contains('#'))
            {
                string[] splitted = AddFriendInput.Split('#');
                App.WebSocketService.AddFriend(splitted[0], splitted[1]);
            }
            else
                LabelMessage = "Enter a valid nickname!";

            AddFriendInput = "";
        }

        private void sendMessage()
        {
            if (MessageContent != null && MessageContent.Length > 0)
            {
                int receiverID = Contacts[SelectedFriendIndex].ID;
                App.WebSocketService.SendMessage(receiverID, MessageContent);

                MessageContent = "";
            }
        }
    }
}
