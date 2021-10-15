using KihonEngine.GameEngine;
using KihonEngine.GameEngine.State;
using KihonEngine.Services;
using KihonEngine.Studio.Helpers;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace KihonEngine.Studio.Controls.ModelEditors
{
    /// <summary>
    /// Interaction logic for FloorModelEditor.xaml
    /// </summary>
    public partial class MapPropertiesEditor : UserControl, ISynchronizedIO
    {
        private ILogService LogService
            => Container.Get<ILogService>();
        private IGameEngineController GameEngineController
            => Container.Get<IGameEngineController>();
        private IGameEngineState State
            => Container.Get<IGameEngineState>();

        public MapPropertiesEditor()
        {
            InitializeComponent();
        }

        public void Synchronize(IGameEngineState state)
        {
            propertyGrid.IsEnabled = state.EngineMode == EngineMode.EditorMode;

            if (propertyGrid.IsEnabled)
            {
                tbName.Text = state.Graphics.LevelName.ToString();
                tbRespawnPositionX.Text = state.Graphics.RespawnPosition.X.ToString();
                tbRespawnPositionY.Text = state.Graphics.RespawnPosition.Y.ToString();
                tbRespawnPositionZ.Text = state.Graphics.RespawnPosition.Z.ToString();
                tbRespawnLookDirectionX.Text = state.Graphics.RespawnLookDirection.X.ToString();
                tbRespawnLookDirectionY.Text = state.Graphics.RespawnLookDirection.Y.ToString();
                tbRespawnLookDirectionZ.Text = state.Graphics.RespawnLookDirection.Z.ToString();
            }
            else
            {
                tbName.Text = string.Empty;
                tbRespawnPositionX.Text = string.Empty;
                tbRespawnPositionY.Text = string.Empty;
                tbRespawnPositionZ.Text = string.Empty;
                tbRespawnLookDirectionX.Text = string.Empty;
                tbRespawnLookDirectionY.Text = string.Empty;
                tbRespawnLookDirectionZ.Text = string.Empty;
            }
        }

        private void tbName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                State.Graphics.LevelName = tbName.Text;
                GameEngineController.NotifyIOs();
            }
        }

        private void tbRespawnPositionX_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var position = State.Graphics.RespawnPosition;

                InputHelper.TryUpdate(
                    tbRespawnPositionX.Text,
                    x => State.Graphics.RespawnPosition = new Point3D(x, position.Y, position.Z));
                
                GameEngineController.NotifyIOs();
            }
        }

        private void tbRespawnPositionY_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var position = State.Graphics.RespawnPosition;
                
                InputHelper.TryUpdate(
                    tbRespawnPositionY.Text,
                    y => State.Graphics.RespawnPosition = new Point3D(position.X, y, position.Z));

                GameEngineController.NotifyIOs();
            }
        }

        private void tbRespawnPositionZ_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var position = State.Graphics.RespawnPosition;
                
                InputHelper.TryUpdate(
                    tbRespawnPositionZ.Text,
                    z => State.Graphics.RespawnPosition = new Point3D(position.X, position.Y, z));

                GameEngineController.NotifyIOs();
            }
        }

        private void tbRespawnLookDirectionX_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var vector = State.Graphics.RespawnLookDirection;

                InputHelper.TryUpdate(
                    tbRespawnLookDirectionX.Text,
                    x => State.Graphics.RespawnLookDirection = new Vector3D(x, vector.Y, vector.Z));

                GameEngineController.NotifyIOs();
            }
        }

        private void tbRespawnLookDirectionY_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var vector = State.Graphics.RespawnLookDirection;

                InputHelper.TryUpdate(
                    tbRespawnLookDirectionY.Text,
                    y => State.Graphics.RespawnLookDirection = new Vector3D(vector.X, y, vector.Z));

                GameEngineController.NotifyIOs();
            }
        }

        private void tbRespawnLookDirectionZ_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var vector = State.Graphics.RespawnLookDirection;

                InputHelper.TryUpdate(
                    tbRespawnLookDirectionZ.Text,
                    z => State.Graphics.RespawnLookDirection = new Vector3D(vector.X, vector.Y, z));

                GameEngineController.NotifyIOs();
            }
        }
    }
}
