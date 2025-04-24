using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;  // Add the dependency injection namespace
using Kooliprojekt.Services;  // Add the services namespace
using KooliProjekt.Data;  // Add the data namespace
using KooliProjekt.WpfApp;  // Add the WPF application namespace

namespace KooliProjekt.WpfApp
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            // Set up the dependency injection container
            var serviceCollection = new ServiceCollection();

            // Register the services and view models for dependency injection
            serviceCollection.AddSingleton<IWorkLogService, WorkLogService>();  // Register the IWorkLogService
            serviceCollection.AddSingleton<MainWindowViewModel>();  // Register the MainWindowViewModel

            // Build the service provider
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        // This is the method that gets called when the application starts
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Create and show the main window with the MainWindowViewModel injected
            var mainWindow = new MainWindow
            {
                DataContext = ServiceProvider.GetService<MainWindowViewModel>()
            };
            mainWindow.Show();  // Show the main window
        }
    }
}
