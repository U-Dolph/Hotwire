using Hotwire.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotwire.ViewModel
{
    public class AddFriendViewModel : BaseViewModel
    {
        private readonly BaseViewModel viewModel;

        public AddFriendViewModel(BaseViewModel viewModel)
        {
            this.viewModel = viewModel;
        }
    }
}
