using AvalonDock.Themes;
using KihonEngine.GameEngine;
using KihonEngine.GameEngine.State;
using KihonEngine.Services;
using Microsoft.Win32;
using System.Windows;
using KihonEngine.Studio.Controls;
using KihonEngine.Studio.Controls.ModelEditors;
using KihonEngine.GameEngine.Graphics.Maps.Predefined;
using KihonEngine.SampleMaps;
using KihonEngine.GameEngine.Graphics;
using System.Windows.Controls;

namespace KihonEngine.Studio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ISynchronizedIO
    {
        private ILogService LogService 
            => Container.Get<ILogService>();
        private IGameEngineController GameEngineController 
            => Container.Get<IGameEngineController>();
        private IGameEngineState State
            => Container.Get<IGameEngineState>();
        private IWorldEngine WorldEngine 
            => Container.Get<IWorldEngine>();        

        public MainWindow()
        {
            InitializeComponent();
            
            dockingManager.Theme = new Vs2013DarkTheme();//new Vs2013LightTheme();
        }

        public void Synchronize(IGameEngineState state)
        {
            Title = $"Kihon Engine Studio [{state.Graphics.LevelName}]";
            menuShowMeterWall.IsChecked = State.Editor.ActionMove.ShowMeterWallWhenMove;
        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeGameEngine();

            lblNotification.Text = $"Load map <{State.Graphics.LevelName}> succeeded";

            var maps = WorldEngine.RegisteredMaps;
            var empty = "Demo : <Empty>";
            menuOpen0.Header = menuOpen1.Header = menuOpen2.Header = menuOpen3.Header = empty;
            menuOpen0.Visibility = menuOpen1.Visibility = menuOpen2.Visibility = menuOpen3.Visibility = Visibility.Collapsed;

            if (maps.Length > 0)
            {
                menuOpen0.Header = "Demo 1 : " + maps[0];
                menuOpen0.Visibility = Visibility.Visible;
            }

            if (maps.Length > 1)
            {
                menuOpen1.Header = "Demo 2 : " + maps[1];
                menuOpen1.Visibility = Visibility.Visible;
            }

            if (maps.Length > 2)
            {
                menuOpen2.Header = "Demo 3 : " + maps[2];
                menuOpen2.Visibility = Visibility.Visible;
            }

            if (maps.Length > 3)
            {
                menuOpen3.Header = "Demo 4 : " + maps[3];
                menuOpen3.Visibility = Visibility.Visible;
            }
        }

        private void MenuOpen_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)sender;
            var indexString = menuItem.Name.Substring(menuItem.Name.Length - 1);
            var index = int.Parse(indexString);

            if (WorldEngine.RegisteredMaps.Length > index)
            {
                GameEngineController.LoadMap(WorldEngine.RegisteredMaps[index]);
                lblNotification.Text = $"Load map <{State.Graphics.LevelName}> succeeded";
            }
        }

        private void MenuNew_Click(object sender, RoutedEventArgs e)
        {
            GameEngineController.LoadMap(PredefinedMapNames.New);
            lblNotification.Text = $"Load map <{State.Graphics.LevelName}> succeeded";
        }

        private void MenuOpenFromFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            dialog.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                GameEngineController.LoadMapFromFile(dialog.FileName);
                lblNotification.Text = $"Load map <{State.Graphics.LevelName}> succeeded (from file {dialog.FileName})";
            }
        }

        private void MenuSaveAs_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            dialog.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (dialog.ShowDialog() == true)
            {
                GameEngineController.SaveMapToFile(dialog.FileName);
                lblNotification.Text = $"Saved successfully to file: <{dialog.FileName}>";
            }
        }

        private void MenuShowMeterWall_Click(object sender, RoutedEventArgs e)
        {
            var show = menuShowMeterWall.IsChecked;
            if (show != State.Editor.ActionMove.ShowMeterWallWhenMove)
            {
                State.Editor.ActionMove.ShowMeterWallWhenMove = show;
            }
        }

        private void MenuMapProperties_Click(object sender, RoutedEventArgs e)
        {
            new MapPropertiesWindow
            {
                Owner = this,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
            }.ShowDialog();
        }

        private void MenuHelp_Click(object sender, RoutedEventArgs e)
        {
            new HelpWindow
            {
                Owner = this,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
            }.ShowDialog();
        }

        private void MenuAbout_Click(object sender, RoutedEventArgs e)
        {
            new AboutWindow
            {
                Owner = this,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
            }.ShowDialog();
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void InitializeGameEngine()
        {
            // Load game
            Engine.Configure();
            
            LogService.AddListener(this.outputWindow);
            LogService.AddListener(new FileLogListener(".out.log"));

            WorldEngine.RegisterMap<RoofTopMapBuilder>();
            WorldEngine.RegisterMap<DarkCastleMapBuilder>();
            WorldEngine.RegisterMap<ArenaMapBuilder>();
            WorldEngine.RegisterMap<LogoMapBuilder>();

            GameEngineController.RegisterIO(this);
            GameEngineController.RegisterIO(toolbox3d);
            GameEngineController.RegisterIO(modelEditor);
            GameEngineController.RegisterIO(stateProperties);
            GameEngineController.RegisterIO(modelExplorer);
            GameEngineController.RegisterIO(sourceCodeViewer);
            
            viewportHost.AttachViewport(GameEngineController.RegisterDefaultGraphicOutput());

            GameEngineController.SwitchToNormalScreen();
            GameEngineController.SwitchToEditorMode();
            GameEngineController.LoadMap(PredefinedMapNames.New);
        }

        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GameEngineController.StopGameLogic();
        }
    }
}
