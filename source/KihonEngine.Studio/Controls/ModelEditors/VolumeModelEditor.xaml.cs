using KihonEngine.GameEngine;
using KihonEngine.GameEngine.Graphics.ModelDefinitions;
using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using KihonEngine.GameEngine.State;
using KihonEngine.Services;
using KihonEngine.Studio.Controls;
using KihonEngine.Studio.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace KihonEngine.Studio.Controls.ModelEditors
{
    /// <summary>
    /// Interaction logic for FloorModelEditor.xaml
    /// </summary>
    public partial class VolumeModelEditor : UserControl, ISynchronizedIO
    {
        private ILogService LogService
            => Container.Get<ILogService>();
        private IGameEngineController GameEngineController
            => Container.Get<IGameEngineController>();
        private IGameEngineState State
            => Container.Get<IGameEngineState>();

        public VolumeModelEditor()
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
                var metadata = (VolumeMetadata)state.Editor.ActionSelect.SelectedModel.Metadata[ModelType.Volume.ToString()];
                tbXSize.Text = metadata.XSize.ToString();
                tbYSize.Text = metadata.YSize.ToString();
                tbZSize.Text = metadata.ZSize.ToString();
                btTextureFrontImg.Background = ImageHelper.CreateTextureBrush(metadata.TextureFront?.Name);
                btTextureBackImg.Background = ImageHelper.CreateTextureBrush(metadata.TextureBack?.Name);
                tbTextureTopImg.Background = ImageHelper.CreateTextureBrush(metadata.TextureTop?.Name);
                btTextureBottomImg.Background = ImageHelper.CreateTextureBrush(metadata.TextureBottom?.Name);
                btTextureLeftImg.Background = ImageHelper.CreateTextureBrush(metadata.TextureLeft?.Name);
                btTextureRightImg.Background = ImageHelper.CreateTextureBrush(metadata.TextureRight?.Name);
                cbUseBackMaterial.IsChecked = metadata.UseBackMaterial;
            }
            else
            {
                tbXSize.Text = string.Empty;
                tbYSize.Text = string.Empty;
                tbZSize.Text = string.Empty;
                btTextureFrontImg.Background = ImageHelper.CreateTextureBrush(string.Empty);
                btTextureBackImg.Background = ImageHelper.CreateTextureBrush(string.Empty);
                tbTextureTopImg.Background = ImageHelper.CreateTextureBrush(string.Empty);
                btTextureBottomImg.Background = ImageHelper.CreateTextureBrush(string.Empty);
                btTextureLeftImg.Background = ImageHelper.CreateTextureBrush(string.Empty);
                btTextureRightImg.Background = ImageHelper.CreateTextureBrush(string.Empty);
                cbUseBackMaterial.IsChecked = false;
            }

            synchronizing = false;
        }

        private void tbXSize_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var layeredModel = State.Editor.ActionSelect.SelectedModel;
                if (layeredModel != null)
                {
                    var definition = GameEngineController.GetDefinition<VolumeDefinition>(layeredModel);
                    
                    InputHelper.TryUpdate(
                        tbXSize.Text,
                        width => definition.Metadata.XSize = width);

                    GameEngineController.ReplaceModelAndNotify(layeredModel, definition);
                }
            }
        }

        private void tbYSize_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var layeredModel = State.Editor.ActionSelect.SelectedModel;
                if (layeredModel != null)
                {
                    var definition = GameEngineController.GetDefinition<VolumeDefinition>(layeredModel);

                    InputHelper.TryUpdate(
                       tbYSize.Text,
                       height => definition.Metadata.YSize = height);

                    GameEngineController.ReplaceModelAndNotify(layeredModel, definition);
                }
            }
        }

        private void tbZSize_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var layeredModel = State.Editor.ActionSelect.SelectedModel;
                if (layeredModel != null)
                {
                    var definition = GameEngineController.GetDefinition<VolumeDefinition>(layeredModel);

                    InputHelper.TryUpdate(
                       tbZSize.Text,
                       length => definition.Metadata.ZSize = length);

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
                    var definition = GameEngineController.GetDefinition<VolumeDefinition>(layeredModel);
                    definition.Metadata.UseBackMaterial = cbUseBackMaterial.IsChecked.HasValue ? cbUseBackMaterial.IsChecked.Value : false;
                    GameEngineController.ReplaceModelAndNotify(layeredModel, definition);
                }
            }
        }

        private void btTextureTop_Click(object sender, RoutedEventArgs e)
        {
            OnEditTexture(def => def.Metadata.TextureTop, (def, e) => def.Metadata.TextureTop = e);
        }

        private void btTextureBack_Click(object sender, RoutedEventArgs e)
        {
            OnEditTexture(def => def.Metadata.TextureBack, (def, e) => def.Metadata.TextureBack = e);
        }

        private void btTextureLeft_Click(object sender, RoutedEventArgs e)
        {
            OnEditTexture(def => def.Metadata.TextureLeft, (def, e) => def.Metadata.TextureLeft = e);
        }

        private void btTextureFront_Click(object sender, RoutedEventArgs e)
        {
            OnEditTexture(def => def.Metadata.TextureFront, (def, e) => def.Metadata.TextureFront = e);
        }

        private void btTextureRight_Click(object sender, RoutedEventArgs e)
        {
            OnEditTexture(def => def.Metadata.TextureRight, (def, e) => def.Metadata.TextureRight = e);
        }

        private void btTextureBottom_Click(object sender, RoutedEventArgs e)
        {
            OnEditTexture(def => def.Metadata.TextureBottom, (def, e) => def.Metadata.TextureBottom = e);
        }

        private void OnEditTexture(Func<VolumeDefinition, TextureMetadata> textureSelector, Action<VolumeDefinition, TextureMetadata> onTextureChanged)
        {
            var layeredModel = State.Editor.ActionSelect.SelectedModel;

            if (layeredModel != null)
            {
                var definition = GameEngineController.GetDefinition<VolumeDefinition>(layeredModel);

                var dialog = new TextureEditorWindow
                {
                    Owner = Window.GetWindow(this),
                    Texture = textureSelector(definition),
                    ShowInTaskbar = false,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                };

                dialog.OnTextureChanged += (sender, e)
                    => OnChangeTexture<VolumeDefinition>(def => onTextureChanged(def, e));

                dialog.ShowDialog();
            }
        }

        private void OnChangeTexture<TDefinition>(Action<TDefinition> changeTextureAction)
             where TDefinition : ModelBaseDefinition
        {
            var layeredModel = State.Editor.ActionSelect.SelectedModel;
            if (layeredModel != null)
            {
                var definition = GameEngineController.GetDefinition<TDefinition>(layeredModel);
                changeTextureAction(definition);
                GameEngineController.ReplaceModelAndNotify(layeredModel, definition);
            }
        }
    }
}
