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
                    definition.Metadata.Direction = new Vector3D(int.Parse(tbX.Text), vector.Y, vector.Z);
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
                    definition.Metadata.Direction = new Vector3D(vector.X, int.Parse(tbY.Text), vector.Z);
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
                    definition.Metadata.Direction = new Vector3D(vector.X, vector.Y, int.Parse(tbZ.Text));
                    GameEngineController.ReplaceModelAndNotify(layeredModel, definition);
                }
            }
        }
    }
}
