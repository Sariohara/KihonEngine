using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace KihonEngine.Studio.Controls
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();
        }

        private void Synchronize()
        {
            var targetAssembly = Assembly.GetExecutingAssembly();
            var assemblyName = targetAssembly.GetName().Name;
            using (var stream = targetAssembly.GetManifestResourceStream($"{assemblyName}.Content.Help.Manual.html"))
            {
                using (var sr = new StreamReader(stream, Encoding.UTF8))
                {
                    webBrowser.NavigateToString(sr.ReadToEnd());
                }
            }            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Synchronize();
        }

        private void btClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
