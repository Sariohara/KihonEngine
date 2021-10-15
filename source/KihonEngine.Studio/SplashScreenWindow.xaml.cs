using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace KihonEngine.Studio
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreenWindow : Window
    {
        private Action _emptyAction = () => { };

        public SplashScreenWindow()
        {
            InitializeComponent();
        }

        public bool DisplayProgressBar
        {
            get { return progressBar.Visibility == Visibility.Visible; }
            set { progressBar.Visibility = value ? Visibility.Visible : Visibility.Hidden; }
        }

        public string Message
        {
            get { return lblStatus.Text; }
            set 
            { 
                lblStatus.Text = value;    
                lblStatus.Dispatcher.Invoke(DispatcherPriority.Render, _emptyAction);
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
