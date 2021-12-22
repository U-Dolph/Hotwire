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
            if (e.PropertyName == "SelectedFriendIndex" && SelectedFriendIndex >= 0 && Contacts.Count > 0)
            {
                App.WebSocketService.GetMessageWithUser(Contacts[SelectedFriendIndex].ID);
            }
        }

        private void disconnect()
        {
            App.WebSocketService.DisconnectFromServer();
            Contacts.Clear();
            Messages.Clear();
            SelectedFriendIndex = 0;
            viewModel.SelectedViewModel = new LoginViewModel(viewModel);
        }

        private void propertiesChanged(object sender, PropertyChangedEventArgs e)
        {
            if (App.WebSocketService.Connected)
            {
                if (e.PropertyName == "Friends" && App.WebSocketService.Friends.Count > 0)
                {
                    App.Current.Dispatcher.Invoke(delegate
                    {
                        Contacts.Clear();

                        foreach (User item in App.WebSocketService.Friends)
                            Contacts.Add(new ContactItem(item.ID, item.Nickname, item.NicknameID, item.LastMessage, item.LastMessageID));

                        Contacts = new ObservableCollection<ContactItem>(Contacts.OrderByDescending(x => x.MessageID));

                        if (SelectedFriendIndex >= 0 && !App.WebSocketService.MessagesRequested)
                        {
                            App.WebSocketService.GetMessageWithUser(Contacts[SelectedFriendIndex].ID);
                            App.WebSocketService.MessagesRequested = true;
                        }
                    });
                }
                else if (e.PropertyName == "Response")
                {
                    LabelMessage = App.WebSocketService.Response;
                }
                else if (e.PropertyName == "CurrentUser")
                {
                    Nickname = $"{App.WebSocketService.CurrentUser.Nickname}#{App.WebSocketService.CurrentUser.NicknameID}";
                }
                else if (e.PropertyName == "FlipFlop")
                {
                    App.Current.Dispatcher.Invoke(delegate
                    {
                        Messages.Clear();

                        foreach (var item in App.WebSocketService.MessageDictionary[Contacts[SelectedFriendIndex].Nickname])
                            Messages.Add(new Message
                            {
                                ID = item.ID,
                                SenderID = item.SenderID,
                                ReciverID = item.ReciverID,
                                Content = item.Content,
                                Nickname = item.Nickname,
                                Aligment = Nickname == item.Nickname ? HorizontalAlignment.Right : HorizontalAlignment.Left
                            });

                        foreach (var item in Contacts)
                        {
                            if (App.WebSocketService.MessageDictionary.ContainsKey(item.Nickname))
                            {
                                if (App.WebSocketService.MessageDictionary[item.Nickname].Count == 0)
                                {
                                    item.LastMessage = "You didn't speak yet";
                                    item.MessageID = 0;
                                }
                                else
                                {
                                    item.LastMessage = App.WebSocketService.MessageDictionary[item.Nickname].LastOrDefault().Content;
                                    item.MessageID = App.WebSocketService.MessageDictionary[item.Nickname].LastOrDefault().ID;
                                }
                            }
                        }

                        Contacts = new ObservableCollection<ContactItem>(Contacts.OrderByDescending(x => x.MessageID));
                    });
                }
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
            if (MessageContent != null && MessageContent.Length > 0 && SelectedFriendIndex >= 0)
            {
                int receiverID = Contacts[SelectedFriendIndex].ID;
                App.WebSocketService.SendMessage(receiverID, MessageContent);

                Contacts[SelectedFriendIndex].LastMessage = MessageContent;

                MessageContent = "";
            }
        }
    }
}
