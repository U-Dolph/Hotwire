using System.ComponentModel;
using System.Windows;

namespace Hotwire.ViewModel.Base
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public RelayCommand ExitCommand { get; }
        public RelayCommand MaximizeCommand { get; }
        public RelayCommand MinimizeCommand { get; }

        private object selectedViewModel;
        private WindowState windowState;

        public object SelectedViewModel
        {
            get
            {
                return selectedViewModel;
            }
            set
            {
                selectedViewModel = value;
                OnPropertyChanged("SelectedViewModel");
            }
        }

        public WindowState WindowState
        {
            get { return windowState; }
            set
            {
                windowState = value;
                OnPropertyChanged("WindowState");
            }
        }

        public BaseViewModel()
        {
            windowState = WindowState.Normal;
            MaximizeCommand = new RelayCommand(maximize);
            MinimizeCommand = new RelayCommand(minimize);
            ExitCommand = new RelayCommand(exit);
        }

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        private void exit()
        {
            Application.Current.Shutdown();
        }

        private void maximize()
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized; 
        }

        private void minimize()
        {
            WindowState = WindowState.Minimized;
        }
    }
}
