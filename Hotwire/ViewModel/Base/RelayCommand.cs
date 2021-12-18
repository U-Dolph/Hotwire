using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Hotwire.ViewModel.Base
{
    public class RelayCommand : ICommand
    {
        private Action action;
        public event EventHandler CanExecuteChanged = (sender, e) => { };
        public RelayCommand(Action action) => this.action = action;
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => action();
    }
}
