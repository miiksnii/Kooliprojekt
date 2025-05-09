using KooliProjekt.WpfApp;
using System.Windows;

namespace KooliProjekt.WpfApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(); // Set DataContext to ViewModel
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = new MainWindowViewModel();
            viewModel.ConfirmDelete = _ =>
            {
                var result = MessageBox.Show(
                                "Are you sure you want to delete selected item?",
                                "Delete list",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Stop
                                );
                return (result == MessageBoxResult.Yes);
            };

            DataContext = viewModel;

            await viewModel.Load();
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
