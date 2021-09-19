using KihonEngine.GameEngine;
using KihonEngine.GameEngine.Graphics.ModelDefinitions;
using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using KihonEngine.GameEngine.State;
using KihonEngine.Services;
using KihonEngine.Studio.Controls;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace KihonEngine.Studio.Controls.ModelEditors
{
    /// <summary>
    /// Interaction logic for FloorModelEditor.xaml
    /// </summary>
    public partial class SkyboxModelEditor : UserControl, ISynchronizedIO
    {
        private ILogService LogService
            => Container.Get<ILogService>();
        private IGameEngineController GameEngineController
            => Container.Get<IGameEngineController>();
        private IGameEngineState State
            => Container.Get<IGameEngineState>();

        public SkyboxModelEditor()
        {
            InitializeComponent();

            cbName.ItemsSource = GetSkyboxes();
            cbName.SelectedIndex = 0;
        }

        private bool synchronizing;
        public void Synchronize(IGameEngineState state)
        {
            synchronizing = true;
            propertyGrid.IsEnabled = state.EngineMode == EngineMode.EditorMode && state.Editor.ActionSelect.SelectedModel != null;

            if (propertyGrid.IsEnabled)
            {
                var metadata = (SkyboxMetadata)state.Editor.ActionSelect.SelectedModel.Metadata[ModelType.Skybox.ToString()];
                TrySelectName(metadata.Name);
                tbWidth.Text = metadata.Width.ToString();
                tbNormalX.Text = metadata.Normal.X.ToString();
                tbNormalY.Text = metadata.Normal.Y.ToString();
                tbNormalZ.Text = metadata.Normal.Z.ToString();
                cbUseBackMaterial.IsChecked = metadata.UseBackMaterial;
            }
            else
            {
                TrySelectName(string.Empty);
                tbWidth.Text = string.Empty;
                tbNormalX.Text = string.Empty;
                tbNormalY.Text = string.Empty;
                tbNormalZ.Text = string.Empty;
                cbUseBackMaterial.IsChecked = false;
            }

            synchronizing = false;
        }

        private bool _disableNameChange;
        private void cbName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!synchronizing && !_disableNameChange && this.IsLoaded)
            {
                var layeredModel = State.Editor.ActionSelect.SelectedModel;
                if (layeredModel != null)
                {
                    var definition = GameEngineController.GetDefinition<SkyboxDefinition>(layeredModel);
                    definition.Metadata.Name = ((SkyboxViewModel)cbName.SelectedItem).Name;
                    GameEngineController.ReplaceModelAndNotify(layeredModel, definition);
                }
            }
        }

        private void TrySelectName(string name)
        {
            var list = GetSkyboxes();
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].Name == name)
                {
                    _disableNameChange = true;
                    cbName.SelectedIndex = i;
                    _disableNameChange = false;
                }
            }
        }

        private void tbWidth_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var layeredModel = State.Editor.ActionSelect.SelectedModel;
                if (layeredModel != null)
                {
                    var definition = GameEngineController.GetDefinition<SkyboxDefinition>(layeredModel);

                    InputHelper.TryUpdate(
                        tbWidth.Text,
                        width => definition.Metadata.Width = width);
                    ;
                    GameEngineController.ReplaceModelAndNotify(layeredModel, definition);
                }
            }
        }

        private void tbNormalX_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var layeredModel = State.Editor.ActionSelect.SelectedModel;
                if (layeredModel != null)
                {
                    var definition = GameEngineController.GetDefinition<SkyboxDefinition>(layeredModel);
                    var vector = definition.Metadata.Normal;
                    InputHelper.TryUpdate(
                        tbNormalX.Text,
                        x => definition.Metadata.Normal = new Vector3D(x, vector.Y, vector.Z));

                    GameEngineController.ReplaceModelAndNotify(layeredModel, definition);
                }
            }
        }

        private void tbNormalY_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var layeredModel = State.Editor.ActionSelect.SelectedModel;
                if (layeredModel != null)
                {
                    var definition = GameEngineController.GetDefinition<SkyboxDefinition>(layeredModel);
                    var vector = definition.Metadata.Normal;

                    InputHelper.TryUpdate(
                        tbNormalY.Text,
                        y => definition.Metadata.Normal = new Vector3D(vector.X, y, vector.Z));

                    GameEngineController.ReplaceModelAndNotify(layeredModel, definition);
                }
            }
        }

        private void tbNormalZ_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var layeredModel = State.Editor.ActionSelect.SelectedModel;
                if (layeredModel != null)
                {
                    var definition = GameEngineController.GetDefinition<SkyboxDefinition>(layeredModel);
                    var vector = definition.Metadata.Normal;

                    InputHelper.TryUpdate(
                        tbNormalZ.Text,
                        z => definition.Metadata.Normal = new Vector3D(vector.X, vector.Y, z));

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
                    var definition = GameEngineController.GetDefinition<SkyboxDefinition>(layeredModel);
                    definition.Metadata.UseBackMaterial = cbUseBackMaterial.IsChecked.HasValue ? cbUseBackMaterial.IsChecked.Value : false;
                    GameEngineController.ReplaceModelAndNotify(layeredModel, definition);
                }
            }
        }

        public class SkyboxViewModel
        {
            public string Name { get; set; }
            public Brush PreviewBrush { get; set; }
        }

        private SkyboxViewModel CreateSkyboxViewModel(string name)
        {
            var result = new SkyboxViewModel { Name = name };

            if (string.IsNullOrEmpty(name))
            {
                result.PreviewBrush = new SolidColorBrush(Colors.Transparent);
            }
            else
            {
                result.PreviewBrush = new ImageBrush(ImageHelper.Get($"Skyboxes.{name}-full.png"));
            }

            return result;
        }

        private SkyboxViewModel[] GetSkyboxes()
        {
            return new[] { string.Empty, "sky0", "sky1", "sky2", "sky3" }
                .Select(x => CreateSkyboxViewModel(x))
                .ToArray();
        }
    }
}
