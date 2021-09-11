namespace KihonEngine.SampleGame
{
    using System.Windows;
    using System.Windows.Input;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Configure game engine
            Engine.Configure();

            // Configure game : we will use an existing map from KihonEngine.SampleMaps
            StandardStartups.BuildStandaloneFullScreenGame<KihonEngine.SampleMaps.DarkCastleMapBuilder>();

            // When click Start button
            btnStart.Click += (sender, e) => Engine.Play();

            // When click Exit button
            btnExit.Click += (sender, e) => Close();

            // When ESC pressed on this window
            KeyDown += (sender, e) => { if (e.Key == Key.Escape) { Close(); } };
        }
    }
}
