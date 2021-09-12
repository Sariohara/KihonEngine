using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace KihonEngine.Studio.Controls
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        private const string MinimalContent =
@"
Minimal help \r\n
----------------\r\n
This file explains how to start with Game Engine Studio\r\n
\r\n
Global comands \r\n
----------------\r\n
F11 to swith with full screen mode\r\n
\r\n
Edit mode\r\n
----------------\r\n
 Select by left click, \r\n
Move by press X, Y or Z when left click pressed\r\n
\r\n
Game mode  \r\n
----------------\r\n
Move with E, S, D, F. \r\n
View by using mouse,\r\n
ESC for quit and back to edit mode\r\n";

        private const string MinimalStyle = @"
<style>
	body {
		background-color: #252525;
		color:lightgray;
		font-family:arial
	}
</style/>
";
        public HelpWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var htmlContent = MinimalStyle + MinimalContent
                .Replace("\\r\\n", "<br/>")
                .Replace("----------------", "<hr/>");

            var filepath = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                @"Content\Help\Manual.html");

            if (System.IO.File.Exists(filepath))
            {
                htmlContent = System.IO.File.ReadAllText(filepath);
            }

            webBrowser.NavigateToString(htmlContent);
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
