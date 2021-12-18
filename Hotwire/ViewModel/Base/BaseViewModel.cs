using System.ComponentModel;

namespace Hotwire.ViewModel.Base
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public RelayCommand ExitCommand { get; }
        private object selectedViewModel;

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

        public BaseViewModel()
        {
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
            System.Windows.Application.Current.Shutdown();
        }
    }
}
