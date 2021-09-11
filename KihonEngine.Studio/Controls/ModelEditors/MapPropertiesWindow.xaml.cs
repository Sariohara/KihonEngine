using KihonEngine.GameEngine;
using KihonEngine.GameEngine.State;
using KihonEngine.Services;
using System.Windows;
using System.Windows.Input;

namespace KihonEngine.Studio.Controls.ModelEditors
{
    /// <summary>
    /// Interaction logic for MapPropertiesWindow.xaml
    /// </summary>
    public partial class MapPropertiesWindow : Window
    {
        private ILogService LogService
            => Container.Get<ILogService>();
        private IGameEngineController GameEngineController
            => Container.Get<IGameEngineController>();
        private IGameEngineState State
            => Container.Get<IGameEngineState>();

        public MapPropertiesWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mapPropertiesEditor.Synchronize(State);
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
