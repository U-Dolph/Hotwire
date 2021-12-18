using Hotwire.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotwire.ViewModel
{
    public class RegisterViewModel : BaseViewModel
    {
        public RelayCommand SwitchToLoginPageCommand { get; }
        private readonly BaseViewModel _viewModel;

        public RegisterViewModel(BaseViewModel viewModel)
        {
            _viewModel = viewModel;
            SwitchToLoginPageCommand = new RelayCommand(switchToLoginPage);
        }

        private void switchToLoginPage()
        {
            _viewModel.SelectedViewModel = new LoginViewModel(_viewModel);
        }
    }
}
