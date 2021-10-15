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

            // Register additional textures and skyboxes
            Engine.RegisterContentFromAssembly(typeof(SampleMaps.E1M1MapBuilder));

            // When click a Start button
            btnStart1.Click += (sender, e) => Engine.Play<SampleMaps.E1M1MapBuilder>();
            btnStart2.Click += (sender, e) => Engine.Play<SampleMaps.Q3DM1MapBuilder>();
            btnStart3.Click += (sender, e) => Engine.Play<SampleMaps.DarkCastleM2MapBuilder>();
            btnStart4.Click += (sender, e) => Engine.Play<SampleMaps.Maze.MazeMapBuilder>();

            // When click Exit button
            btnExit.Click += (sender, e) => Close();

            // When ESC pressed on this window
            KeyDown += (sender, e) => { if (e.Key == Key.Escape) { Close(); } };
        }
    }
}
