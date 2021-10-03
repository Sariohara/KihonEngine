namespace KihonEngine.SampleGame
{
    using System.Windows;
    using System.Windows.Input;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Configure game
            StandardStartups.BuildStandaloneFullScreenGame();

            // When click Start button
            btnStart.Click += (sender, e) => Engine.Play<SampleMaps.DarkCastleM2MapBuilder>();

            // When click Exit button
            btnExit.Click += (sender, e) => Close();

            // When ESC pressed on this window
            KeyDown += (sender, e) => { if (e.Key == Key.Escape) { Close(); } };
        }
    }
}
