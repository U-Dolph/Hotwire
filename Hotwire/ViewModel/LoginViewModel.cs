using Hotwire.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotwire.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public RelayCommand SwitchToRegisterPageCommand { get; }
        public RelayCommand SwitchToChatPageCommand { get; }
        private readonly BaseViewModel viewModel;

        public LoginViewModel(BaseViewModel viewModel)
        {
            this.viewModel = viewModel;
            SwitchToRegisterPageCommand = new RelayCommand(switchToRegisterPage);
            SwitchToChatPageCommand = new RelayCommand(switchToChatPage);
        }

        private void switchToRegisterPage()
        {
            viewModel.SelectedViewModel = new RegisterViewModel(viewModel); 
        }

        private void switchToChatPage()
        {
            viewModel.SelectedViewModel = new ChatViewModel(viewModel);
        }
    }
}
