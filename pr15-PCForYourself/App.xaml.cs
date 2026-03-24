using System.Windows;
using pr15_PCForYourself.ViewModels;
using pr15_PCForYourself.Views;

namespace pr15_PCForYourself
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainVM = new MainViewModel(); // или ManViewModel
            var mainWindow = new MainWindow();
            mainWindow.DataContext = mainVM;
            mainWindow.Show();
        }
    }
}