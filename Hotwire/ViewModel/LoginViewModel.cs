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

        public LoginViewModel(BaseViewModel viewModel)
        {
            this.viewModel = viewModel;
            LabelMessage = "Cigány";
            SwitchToRegisterPageCommand = new RelayCommand(switchToRegisterPage);
            SwitchToChatPageCommand = new RelayCommand(login);
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
            //RegisterButtonEnabled = false;
            LabelMessage = " ";

            if (Password == null  || Password.Length < 1 )
                LabelMessage = "Please enter your password";
            else if (Username == null || Username.Length < 1)
                LabelMessage = "Please enter your username";
            else
                LabelMessage = await App.HttpService.LoginUser(new User(Username, Password, StayLoggedIn));

            //RegisterButtonEnabled = true;
        }
    }
}
