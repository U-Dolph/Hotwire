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
        public RelayCommand SwitchToAddFriendPageCommand { get; }
        private readonly BaseViewModel viewModel;

        public ChatViewModel(BaseViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        private void switchToAddFriendPage()
        {
            viewModel.SelectedViewModel = new AddFriendViewModel(viewModel);
        }
    }
}
