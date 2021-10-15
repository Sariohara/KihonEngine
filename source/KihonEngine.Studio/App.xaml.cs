using KihonEngine.GameEngine;
using KihonEngine.GameEngine.Graphics;
using KihonEngine.GameEngine.Graphics.Content;
using KihonEngine.GameEngine.Graphics.Maps.Predefined;
using KihonEngine.SampleMaps;
using KihonEngine.Services;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace KihonEngine.Studio
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IGameEngineController GameEngineController
            => Container.Get<IGameEngineController>();
        private IWorldEngine WorldEngine
            => Container.Get<IWorldEngine>();
        private IContentService ContentService
            => Container.Get<IContentService>();

        private SplashScreenWindow _splashScreen;

        public void OnStartup(object sender, StartupEventArgs e)
        {
            DisplaySplashScreen();

            InitializeGameEngine();

            InitializeGameEngineGraphics();

            InitializeMainWindow();
        }

        private void DisplayMessage(string message)
        {
            _splashScreen.Message = message;
        }

        private void DisplaySplashScreen()
        {
            _splashScreen = new SplashScreenWindow
            {
                ShowInTaskbar = false,
                DisplayProgressBar = false,
            };

            this.MainWindow = _splashScreen;
            _splashScreen.Show();
        }

        private void InitializeGameEngine()
        {
            // Load game
            DisplayMessage("Start Game engine...");
            Engine.Configure();

            //LogService.AddListener(new FileLogListener(".out.log"));

            DisplayMessage("Register content sources...");
            ContentService.RegisterSource(new EmbeddedContentSource(typeof(E1M1MapBuilder)));

            DisplayMessage("Register maps...");
            WorldEngine.RegisterMap<E1M1MapBuilder>();
            WorldEngine.RegisterMap<Q3DM1MapBuilder>();
            //WorldEngine.RegisterMap<MazeMapBuilder>();
            //            WorldEngine.RegisterMap<DarkCastleMapBuilder>();
            WorldEngine.RegisterMap<DarkCastleM2MapBuilder>();
            WorldEngine.RegisterMap<RoofTopMapBuilder>();
            WorldEngine.RegisterMap<LogoMapBuilder>();
            WorldEngine.RegisterMap<DarkCastleM3MapBuilder>();
        }

        private void InitializeGameEngineGraphics()
        {
            DisplayMessage("Prepare 3D graphics output...");
            GameEngineController.RegisterDefaultGraphicOutput();

            GameEngineController.SwitchToNormalScreen();
            GameEngineController.SwitchToEditorMode();

            DisplayMessage("Load default map...");
            GameEngineController.LoadMap(PredefinedMapNames.New);
        }

        private void InitializeMainWindow()
        {
            DisplayMessage("Start IDE...");
            var mainWindow = new MainWindow();
            this.MainWindow = mainWindow;

            _splashScreen.Topmost = true;
            mainWindow.Show();
            _splashScreen.Owner = mainWindow;
            _splashScreen.Topmost = false;

            mainWindow.ContentRendered += (sender, e) =>
            {
                mainWindow.Owner = null;
                _splashScreen.Close();
            };
        }
    }
}
