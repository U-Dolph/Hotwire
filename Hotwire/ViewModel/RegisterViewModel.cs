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
        private readonly BaseViewModel viewModel;

        public RegisterViewModel(BaseViewModel viewModel)
        {
            this.viewModel = viewModel;
            SwitchToLoginPageCommand = new RelayCommand(switchToLoginPage);
        }

        private void switchToLoginPage()
        {
            viewModel.SelectedViewModel = new LoginViewModel(viewModel);
        }
    }
}
