using KihonEngine.Studio.Helpers;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

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
            lnkDocumentation.NavigateUri = new System.Uri(ExternalResources.DocumentationUrl);
            lnkDocumentation.ToolTip = ExternalResources.DocumentationUrl;

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

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            ExternalResources.Navigate(e.Uri.AbsoluteUri);
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
