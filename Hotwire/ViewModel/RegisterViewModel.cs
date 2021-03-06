using Hotwire.Model;
using Hotwire.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Hotwire.ViewModel
{
    public class RegisterViewModel : BaseViewModel
    {
        public RelayCommand SwitchToLoginPageCommand { get; }
        public RelayCommand RegisterCommand { get; }

        private readonly BaseViewModel viewModel;

        public string Username { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
        public bool IsBusy { get; set; }

        public string LabelMessage { get; private set; }

        public bool RegisterButtonEnabled { get; private set; }

        public Visibility ButtonVisibility
        {
            get { return IsBusy == true ? Visibility.Hidden : Visibility.Visible; }
        }

        public RegisterViewModel(BaseViewModel viewModel)
        {
            this.viewModel = viewModel;

            SwitchToLoginPageCommand = new RelayCommand(switchToLoginPage);
            RegisterCommand = new RelayCommand(registerUser);

            LabelMessage = " ";
            RegisterButtonEnabled = true;
            IsBusy = false;
        }

        private void switchToLoginPage()
        {
            viewModel.SelectedViewModel = new LoginViewModel(viewModel);
        }

        private async void registerUser()
        {
            IsBusy = true;
            RegisterButtonEnabled = false;
            LabelMessage = " ";

            if (Username == null || Username.Length < 3)
                LabelMessage = "The username must be longer than 5 characters!";
            else if (Nickname == null || Nickname.Length < 3)
                LabelMessage = "The nickname must be longer than 5 characters!";
            else if (Password == null || RePassword == null || Password != RePassword || Password.Length < 1 || RePassword.Length < 1)
                LabelMessage = "Passwords must be the same!";
            else
                LabelMessage = await App.HttpService.RegisterUser(new User(Username, Nickname, Password));

            RegisterButtonEnabled = true;
            IsBusy = false;
        }
    }
}
