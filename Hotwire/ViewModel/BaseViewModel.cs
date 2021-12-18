using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotwire.Core;

namespace Hotwire.ViewModel
{
    public class BaseViewModel : ObservableObject
    {
        public LoginViewModel LoginVM { get; set; }
        public RelayCommand ExitCommand { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public BaseViewModel()
        {
            LoginVM = new LoginViewModel();
            CurrentView = LoginVM;
            ExitCommand = new RelayCommand(o =>
            {
                System.Windows.Application.Current.Shutdown();
            });
        }
    }
}
