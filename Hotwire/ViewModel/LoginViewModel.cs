using Hotwire.Model;
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

        public string LabelMessage { get; private set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool StayLoggedIn { get; set; }

        public bool LoginButtonEnabled { get; set; }

        public LoginViewModel(BaseViewModel viewModel)
        {
            this.viewModel = viewModel;
            LabelMessage = " ";
            SwitchToRegisterPageCommand = new RelayCommand(switchToRegisterPage);
            SwitchToChatPageCommand = new RelayCommand(login);

            LoginButtonEnabled = true;
        }

        private void switchToRegisterPage()
        {
            viewModel.SelectedViewModel = new RegisterViewModel(viewModel); 
        }

        private void switchToChatPage()
        {
            viewModel.SelectedViewModel = new ChatViewModel(viewModel);
        }
        private async void login()
        {
            LoginButtonEnabled = false;
            LabelMessage = " ";

            if (Username == null || Username.Length < 1)
                LabelMessage = "Please enter your username";
            else if (Password == null || Password.Length < 1 )
                LabelMessage = "Please enter your password";

            else
                LabelMessage = await App.HttpService.LoginUser(new User(Username, Password, StayLoggedIn));

            LoginButtonEnabled = true;
        }
    }
}
