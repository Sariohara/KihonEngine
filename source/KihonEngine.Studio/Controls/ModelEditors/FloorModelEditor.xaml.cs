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

namespace KihonEngine.Studio.Controls.ModelEditors
{
    /// <summary>
    /// Interaction logic for FloorModelEditor.xaml
    /// </summary>
    public partial class FloorModelEditor : UserControl, ISynchronizedIO
    {
        private ILogService LogService
            => Container.Get<ILogService>();
        private IGameEngineController GameEngineController
            => Container.Get<IGameEngineController>();
        private IGameEngineState State
            => Container.Get<IGameEngineState>();

        public FloorModelEditor()
        {
            InitializeComponent();

            cbTexture.ItemsSource = GetTextures();
            cbTexture.SelectedIndex = 0;
        }

        private bool synchronizing;
        public void Synchronize(IGameEngineState state)
        {
            synchronizing = true;
            propertyGrid.IsEnabled = state.EngineMode == EngineMode.EditorMode && state.Editor.ActionSelect.SelectedModel != null;

            if (propertyGrid.IsEnabled)
            {
                var metadata = (FloorMetadata)state.Editor.ActionSelect.SelectedModel.Metadata[ModelType.Floor.ToString()];
                tbWidth.Text = metadata.Width.ToString();
                tbLength.Text = metadata.Length.ToString();
                TrySelectTexture(metadata.Texture);
                cbUseBackMaterial.IsChecked = metadata.UseBackMaterial;
            }
            else
            {
                tbWidth.Text = string.Empty;
                tbLength.Text = string.Empty;
                TrySelectTexture(string.Empty);
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
                    var definition = GameEngineController.GetDefinition<FloorDefinition>(layeredModel);

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
                    var definition = GameEngineController.GetDefinition<FloorDefinition>(layeredModel);

                    InputHelper.TryUpdate(
                        tbLength.Text,
                        length => definition.Metadata.Length = length);

                    GameEngineController.ReplaceModelAndNotify(layeredModel, definition);
                }
            }
        }

        private bool _disableTextureChange;
        private void cbtexture_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!synchronizing && !_disableTextureChange && this.IsLoaded)
            {
                var layeredModel = State.Editor.ActionSelect.SelectedModel;
                if (layeredModel != null)
                {
                    var definition = GameEngineController.GetDefinition<FloorDefinition>(layeredModel);
                    definition.Metadata.Texture = ((TextureViewModel)cbTexture.SelectedItem).Name;
                    GameEngineController.ReplaceModelAndNotify(layeredModel, definition);
                }
            }
        }

        private void TrySelectTexture(string name)
        {
            var textures = GetTextures();
            for(int i = 0; i < textures.Length; i++)
            {
                if (textures[i].Name == name)
                {
                    _disableTextureChange = true;
                    cbTexture.SelectedIndex = i;
                    _disableTextureChange = false;
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
                    var definition = GameEngineController.GetDefinition<FloorDefinition>(layeredModel);
                    definition.Metadata.UseBackMaterial = cbUseBackMaterial.IsChecked.HasValue ? cbUseBackMaterial.IsChecked.Value : false;
                    GameEngineController.ReplaceModelAndNotify(layeredModel, definition);
                }
            }
        }

        private class TextureViewModel
        {
            public string Name { get; set; }
            public Brush PreviewBrush { get; set; }
        }

        private TextureViewModel CreateTextureViewModel(string filename)
        {
            var result = new TextureViewModel { Name = filename };

            if (string.IsNullOrEmpty(filename))
            {
                result.PreviewBrush = new SolidColorBrush(Colors.Transparent);
            }
            else
            {
                result.PreviewBrush = new ImageBrush(ImageHelper.Get($"Textures.{filename}"));
            }

            return result;
        }

        private TextureViewModel[] GetTextures()
        {
            return new[] { string.Empty, "default.png", "floor0.jpg", "ki.png", "hon.png" }
                .Select(x => CreateTextureViewModel(x))
                .ToArray();
        }
    }
}
