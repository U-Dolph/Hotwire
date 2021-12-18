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
        private readonly BaseViewModel _viewModel;

        public LoginViewModel(BaseViewModel viewModel)
        {
            _viewModel = viewModel;
            SwitchToRegisterPageCommand = new RelayCommand(switchToRegisterPage);
        }

        private void switchToRegisterPage()
        {
            _viewModel.SelectedViewModel = new RegisterViewModel(_viewModel); 
        }
    }
}
