using KooliProjekt.WpfApp;
using System.Windows;

namespace KooliProjekt.WpfApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();  // This initializes the XAML UI components
            Loaded += MainWindow_Loaded;
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
    }
}
