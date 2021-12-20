using Hotwire.Model;
using Hotwire.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotwire.ViewModel
{
    public class ChatViewModel : BaseViewModel
    {
        public ObservableCollection<ContactItem> Contacts { get; set; }
        public RelayCommand DisconnectCommand { get; }
        private readonly BaseViewModel viewModel;

        public ChatViewModel(BaseViewModel viewModel)
        {
            this.viewModel = viewModel;
            DisconnectCommand = new RelayCommand(disconnect);
            Contacts = new ObservableCollection<ContactItem>();

            App.WebSocketService.GetFriends();
            foreach (var item in App.WebSocketService.Friends)
                Contacts.Add(new ContactItem(item.Nickname, "-"));
        }

        private void disconnect()
        {
            App.WebSocketService.DisconnectFromServer();
            viewModel.SelectedViewModel = new LoginViewModel(viewModel);
        }
    }
}
