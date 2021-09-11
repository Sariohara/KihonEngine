using KihonEngine.GameEngine.State;
using KihonEngine.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KihonEngine.GameEngine.Graphics.Output
{
    /// <summary>
    /// Interaction logic for WorldVisualizeFullScreenWindow.xaml
    /// </summary>
    public partial class WorldVisualizeFullScreenWindow : Window, IGraphicOutput
    {
        private ILogService LogService
            => Container.Get<ILogService>();
        private IGameEngineController GameEngineController
            => Container.Get<IGameEngineController>();

        private bool detaching;

        public WorldVisualizeFullScreenWindow()
        {
            InitializeComponent();
        }

        public void AttachViewport(Viewport3D viewport)
        {
            grid.Children.Add(viewport);
            
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;

            MouseEnter += UserControl_MouseEvents;
            MouseMove += UserControl_MouseEvents;
            MouseDown += UserControl_MouseButtonEvents;
            MouseUp += UserControl_MouseButtonEvents;
            MouseWheel += UserControl_MouseWheel;
            KeyDown += UserControl_KeyEvents;
            KeyUp += UserControl_KeyEvents;
            Closing += Window_Closing;

            if (Container.Get<IGameEngineState>().Game.IsStandaloneFullScreenGame)
            {
                gridTipsMessage.Visibility = Visibility.Hidden;
            }
            else
            {
                gridTipsMessage.Visibility = Visibility.Visible;
            }

            Show();
            viewport.Focus();
        }

        public void DetachViewport(Viewport3D viewport)
        {
            grid.Children.Remove(viewport);

            MouseEnter -= UserControl_MouseEvents;
            MouseMove -= UserControl_MouseEvents;
            MouseDown -= UserControl_MouseButtonEvents;
            MouseUp -= UserControl_MouseButtonEvents;
            MouseWheel -= UserControl_MouseWheel;
            KeyDown -= UserControl_KeyEvents;
            KeyUp -= UserControl_KeyEvents;
            Closing -= Window_Closing;

            gridHeadUpDisplayPlayMode.Visibility = Visibility.Hidden;
            gridHeadUpDisplayEditorMode.Visibility = Visibility.Hidden;

            detaching = true;
            Close();
        }

        public void Synchronize(IGameEngineState state)
        {
            if (state.EngineMode == EngineMode.PlayMode)
            {
                gridHeadUpDisplayPlayMode.Visibility = Visibility.Visible;
                gridHeadUpDisplayEditorMode.Visibility = Visibility.Hidden;
            }

            if (state.EngineMode == EngineMode.EditorMode)
            {
                gridHeadUpDisplayPlayMode.Visibility = Visibility.Hidden;
                gridHeadUpDisplayEditorMode.Visibility = Visibility.Visible;
            }

            if (state.EngineMode == EngineMode.Off)
            {
                gridHeadUpDisplayPlayMode.Visibility = Visibility.Hidden;
                gridHeadUpDisplayEditorMode.Visibility = Visibility.Hidden;
            }
        }

        private void UserControl_MouseEvents(object sender, MouseEventArgs e)
        {
            GameEngineController.DispatchMouseEvent(e);
        }

        public void UserControl_KeyEvents(object sender, KeyEventArgs e)
        {
            GameEngineController.DispatchKeyboardEvent(e);
        }

        private void UserControl_MouseButtonEvents(object sender, MouseButtonEventArgs e)
        {
            GameEngineController.DispatchMouseButtonEvent(e);
        }

        private void UserControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            GameEngineController.DispatchMouseWheelEvent(e);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!detaching)
            {
                e.Cancel = true;
            }
        }
    }
}
