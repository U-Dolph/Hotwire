using Hotwire.ViewModel;
using Hotwire.ViewModel.Base;
using System.Windows;

namespace Hotwire
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var viewModel = new BaseViewModel();
            viewModel.SelectedViewModel = new LoginViewModel(viewModel);
            DataContext = viewModel;
        }
    }
}
