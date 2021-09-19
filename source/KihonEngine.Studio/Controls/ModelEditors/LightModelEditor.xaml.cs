using KihonEngine.GameEngine;
using KihonEngine.GameEngine.Graphics.ModelDefinitions;
using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using KihonEngine.GameEngine.State;
using KihonEngine.Services;
using KihonEngine.Studio.Controls;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace KihonEngine.Studio.Controls.ModelEditors
{
    /// <summary>
    /// Interaction logic for FloorModelEditor.xaml
    /// </summary>
    public partial class LightModelEditor : UserControl, ISynchronizedIO
    {
        private ILogService LogService
            => Container.Get<ILogService>();
        private IGameEngineController GameEngineController
            => Container.Get<IGameEngineController>();
        private IGameEngineState State
            => Container.Get<IGameEngineState>();

        public LightModelEditor()
        {
            InitializeComponent();
        }

        public void Synchronize(IGameEngineState state)
        {
            propertyGrid.IsEnabled = state.EngineMode == EngineMode.EditorMode && state.Editor.ActionSelect.SelectedModel != null;

            if (propertyGrid.IsEnabled)
            {
                var metadata = (LightMetadata)state.Editor.ActionSelect.SelectedModel.Metadata[ModelType.Light.ToString()];
                tbX.Text = metadata.Direction.X.ToString();
                tbY.Text = metadata.Direction.Y.ToString();
                tbZ.Text = metadata.Direction.Z.ToString();
            }
            else
            {
                tbX.Text = string.Empty;
                tbY.Text = string.Empty;
                tbZ.Text = string.Empty;
            }
        }

        private void tbX_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var layeredModel = State.Editor.ActionSelect.SelectedModel;
                if (layeredModel != null)
                {
                    var definition = GameEngineController.GetDefinition<LightDefinition>(layeredModel);
                    var vector = definition.Metadata.Direction;

                    InputHelper.TryUpdate(
                        tbX.Text,
                        x => definition.Metadata.Direction = new Vector3D(x, vector.Y, vector.Z));

                    GameEngineController.ReplaceModelAndNotify(layeredModel, definition);
                }
            }
        }

        private void tbY_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var layeredModel = State.Editor.ActionSelect.SelectedModel;
                if (layeredModel != null)
                {
                    var definition = GameEngineController.GetDefinition<LightDefinition>(layeredModel);
                    var vector = definition.Metadata.Direction;
                    
                    InputHelper.TryUpdate(
                        tbY.Text,
                        y => definition.Metadata.Direction = new Vector3D(vector.X, y, vector.Z));


                    GameEngineController.ReplaceModelAndNotify(layeredModel, definition);
                }
            }
        }

        private void tbZ_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var layeredModel = State.Editor.ActionSelect.SelectedModel;
                if (layeredModel != null)
                {
                    var definition = GameEngineController.GetDefinition<LightDefinition>(layeredModel);
                    var vector = definition.Metadata.Direction;
                    
                    InputHelper.TryUpdate(
                        tbZ.Text,
                        z => definition.Metadata.Direction = new Vector3D(vector.X, vector.Y, z));

                    GameEngineController.ReplaceModelAndNotify(layeredModel, definition);
                }
            }
        }
    }
}
