using System.Windows;

namespace KihonEngine.Studio
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void OnStartup(object sender, StartupEventArgs e)
        {
            new MainWindow().Show();
        }
    }
}
