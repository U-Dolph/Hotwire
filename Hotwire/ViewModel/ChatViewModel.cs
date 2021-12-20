using Hotwire.Model;
using Hotwire.ViewModel.Base;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Hotwire.ViewModel
{
    public class ChatViewModel : BaseViewModel
    {
        public ObservableCollection<ContactItem> Contacts { get; set; }
        public RelayCommand DisconnectCommand { get; }
        public RelayCommand AddFriendCommand { get; }
        private readonly BaseViewModel viewModel;

        public string AddFriendInput { get; set; }
        public string LabelMessage { get; set; }
        public string Nickname { get; set; }

        public ChatViewModel(BaseViewModel viewModel)
        {
            this.viewModel = viewModel;
            DisconnectCommand = new RelayCommand(disconnect);
            AddFriendCommand = new RelayCommand(addFriend);
            Contacts = new ObservableCollection<ContactItem>();
            Nickname = "";

            App.WebSocketService.GetFriends();

            LabelMessage = " ";

            App.WebSocketService.PropertyChanged += propertiesChanged;
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
                        Contacts.Add(new ContactItem(item.Nickname,item.NicknameID, "-"));
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
    }
}
