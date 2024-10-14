using System.Configuration;
using System.Data;
using System.Windows;

namespace WPF_MVVM_SPA_Template
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Show splash screen
            var splashScreen = new SplashScreen();
            splashScreen.Show();

            // Simulate loading process (e.g., data initialization, etc.)
            Task.Delay(3000).ContinueWith(t =>
            {
                // Show main window after 3 seconds (on the UI thread)
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var mainWindow = new MainWindow();
                    mainWindow.Show();

                    // Close splash screen
                    splashScreen.Close();
                });
            });
        }
    
    }

}
