using KihonEngine.GameEngine;
using KihonEngine.GameEngine.Graphics.ModelDefinitions;
using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using KihonEngine.GameEngine.State;
using KihonEngine.Services;
using KihonEngine.Studio.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KihonEngine.Studio.Controls.ModelEditors
{
    /// <summary>
    /// Interaction logic for FloorModelEditor.xaml
    /// </summary>
    public partial class CeilingModelEditor : UserControl, ISynchronizedIO
    {
        private ILogService LogService
            => Container.Get<ILogService>();
        private IGameEngineController GameEngineController
            => Container.Get<IGameEngineController>();
        private IGameEngineState State
            => Container.Get<IGameEngineState>();

        public CeilingModelEditor()
        {
            InitializeComponent();
        }

        private bool synchronizing;
        public void Synchronize(IGameEngineState state)
        {
            synchronizing = true;
            propertyGrid.IsEnabled = state.EngineMode == EngineMode.EditorMode && state.Editor.ActionSelect.SelectedModel != null;

            if (propertyGrid.IsEnabled)
            {
                var metadata = (CeilingMetadata)state.Editor.ActionSelect.SelectedModel.Metadata[ModelType.Ceiling.ToString()];
                tbWidth.Text = metadata.Width.ToString();
                tbLength.Text = metadata.Length.ToString();
                cbUseBackMaterial.IsChecked = metadata.UseBackMaterial;
            }
            else
            {
                tbWidth.Text = string.Empty;
                tbLength.Text = string.Empty;
                cbUseBackMaterial.IsChecked = false;
            }

            synchronizing = false;
        }

        private void tbWidth_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var layeredModel = State.Editor.ActionSelect.SelectedModel;
                if (layeredModel != null)
                {
                    var definition = GameEngineController.GetDefinition<CeilingDefinition>(layeredModel);

                    InputHelper.TryUpdate(
                        tbWidth.Text,
                        width => definition.Metadata.Width = width);

                    GameEngineController.ReplaceModelAndNotify(layeredModel, definition);
                }
            }
        }

        private void tbLength_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var layeredModel = State.Editor.ActionSelect.SelectedModel;
                if (layeredModel != null)
                {
                    var definition = GameEngineController.GetDefinition<CeilingDefinition>(layeredModel);

                    InputHelper.TryUpdate(
                        tbLength.Text,
                        length => definition.Metadata.Length = length);

                    GameEngineController.ReplaceModelAndNotify(layeredModel, definition);
                }
            }
        }

        private void cbUseBackMaterial_Checked(object sender, RoutedEventArgs e)
        {
            if (!synchronizing)
            {
                var layeredModel = State.Editor.ActionSelect.SelectedModel;
                if (layeredModel != null)
                {
                    var definition = GameEngineController.GetDefinition<CeilingDefinition>(layeredModel);
                    definition.Metadata.UseBackMaterial = cbUseBackMaterial.IsChecked.HasValue ? cbUseBackMaterial.IsChecked.Value : false;
                    GameEngineController.ReplaceModelAndNotify(layeredModel, definition);
                }
            }
        }
    }
}
