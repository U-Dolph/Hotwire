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
    public class RegisterViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public RelayCommand SwitchToLoginPageCommand { get; }
        public RelayCommand RegisterCommand { get; }

        private readonly BaseViewModel viewModel;

        public string Username { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }

        public string LabelMessage { get; private set; }

        public bool RegisterButtonEnabled { get; private set; }

        public RegisterViewModel(BaseViewModel viewModel)
        {
            this.viewModel = viewModel;

            SwitchToLoginPageCommand = new RelayCommand(switchToLoginPage);
            RegisterCommand = new RelayCommand(registerUser);

            LabelMessage = " ";
            RegisterButtonEnabled = true;
        }

        private void switchToLoginPage()
        {
            viewModel.SelectedViewModel = new LoginViewModel(viewModel);
        }

        private async void registerUser()
        {
            RegisterButtonEnabled = false;
            LabelMessage = " ";

            if (Password == null || RePassword == null || Password != RePassword || Password.Length < 1 || RePassword.Length < 1)
                LabelMessage = "Passwords must be the same!";
            else if (Nickname == null || Nickname.Length < 5)
                LabelMessage = "The nickname must be longer than 5 characters!";
            else if (Username == null || Username.Length < 5)
                LabelMessage = "The username must be longer than 5 characters!";
            else
                LabelMessage = await App.HttpService.RegisterUser(new User(Username, Nickname, Password));

            RegisterButtonEnabled = true;
        }
    }
}
