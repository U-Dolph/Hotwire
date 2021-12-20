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

        public ChatViewModel(BaseViewModel viewModel)
        {
            this.viewModel = viewModel;
            DisconnectCommand = new RelayCommand(disconnect);
            AddFriendCommand = new RelayCommand(addFriend);
            Contacts = new ObservableCollection<ContactItem>();

            App.WebSocketService.GetFriends();

            LabelMessage = " ";

            App.WebSocketService.PropertyChanged += userlistUpdated;
        }

        private void disconnect()
        {
            App.WebSocketService.DisconnectFromServer();
            viewModel.SelectedViewModel = new LoginViewModel(viewModel);
        }

        private void userlistUpdated(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Friends")
            {
                App.Current.Dispatcher.Invoke(delegate
                {
                    Contacts.Clear();

                    foreach (var item in App.WebSocketService.Friends)
                        Contacts.Add(new ContactItem(item.Nickname, "-"));
                });
            }
            else if (e.PropertyName == "Response")
            {
                LabelMessage = App.WebSocketService.Response;
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
