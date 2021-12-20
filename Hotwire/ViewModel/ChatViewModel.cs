using Hotwire.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotwire.ViewModel
{
    public class ChatViewModel : BaseViewModel
    {
        public RelayCommand DisconnectCommand { get; }
        private readonly BaseViewModel viewModel;

        public ChatViewModel(BaseViewModel viewModel)
        {
            this.viewModel = viewModel;
            DisconnectCommand = new RelayCommand(disconnect);
        }

        private void disconnect()
        {
            App.WebSocketService.DisconnectFromServer();
            viewModel.SelectedViewModel = new LoginViewModel(viewModel);
        }
    }
}
