using KihonEngine.GameEngine;
using KihonEngine.GameEngine.Graphics.ModelDefinitions;
using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using KihonEngine.GameEngine.State;
using KihonEngine.Services;
using KihonEngine.Studio.Controls;
using KihonEngine.Studio.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace KihonEngine.Studio.Controls.ModelEditors
{
    /// <summary>
    /// Interaction logic for FloorModelEditor.xaml
    /// </summary>
    public partial class WallModelEditor : UserControl, ISynchronizedIO
    {
        private ILogService LogService
            => Container.Get<ILogService>();
        private IGameEngineController GameEngineController
            => Container.Get<IGameEngineController>();
        private IGameEngineState State
            => Container.Get<IGameEngineState>();

        private List<VolumeFace> _wallFaces;
        public WallModelEditor()
        {
            InitializeComponent();

            _wallFaces = new List<VolumeFace>();
            _wallFaces.AddRange(new[] { VolumeFace.Front, VolumeFace.Back, VolumeFace.Left, VolumeFace.Right });
            cbFaces.ItemsSource = _wallFaces;
        }

        private bool synchronizing;
        public void Synchronize(IGameEngineState state)
        {
            synchronizing = true;
            propertyGrid.IsEnabled = state.EngineMode == EngineMode.EditorMode && state.Editor.ActionSelect.SelectedModel != null;

            if (propertyGrid.IsEnabled)
            {
                var metadata = (WallMetadata)state.Editor.ActionSelect.SelectedModel.Metadata[ModelType.Wall.ToString()];
                tbXSize.Text = metadata.XSize.ToString();
                tbYSize.Text = metadata.YSize.ToString();
                btTextureImg.Background = ImageHelper.CreateTextureBrush(metadata.Texture?.Name);
                cbUseBackMaterial.IsChecked = metadata.UseBackMaterial;
                cbFaces.SelectedIndex = _wallFaces.IndexOf(metadata.Face);
            }
            else
            {
                cbFaces.SelectedIndex = 0;
                tbXSize.Text = string.Empty;
                tbYSize.Text = string.Empty;
                btTextureImg.Background = ImageHelper.CreateTextureBrush(string.Empty);
                cbUseBackMaterial.IsChecked = false;
            }

            synchronizing = false;
        }

        private void cbFaces_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!synchronizing)
            {
                var layeredModel = State.Editor.ActionSelect.SelectedModel;
                if (layeredModel != null)
                {
                    var definition = GameEngineController.GetDefinition<WallDefinition>(layeredModel);
                    var selectedFace = (VolumeFace)cbFaces.SelectedItem;
                    definition.Metadata.Face = selectedFace;
                    GameEngineController.ReplaceModelAndNotify(layeredModel, definition);
                }
            }
        }

        private void tbXSize_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var layeredModel = State.Editor.ActionSelect.SelectedModel;
                if (layeredModel != null)
                {
                    var definition = GameEngineController.GetDefinition<WallDefinition>(layeredModel);

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
                    var definition = GameEngineController.GetDefinition<WallDefinition>(layeredModel);

                    InputHelper.TryUpdate(
                       tbYSize.Text,
                       height => definition.Metadata.YSize = height);

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
                    var definition = GameEngineController.GetDefinition<WallDefinition>(layeredModel);
                    definition.Metadata.UseBackMaterial = cbUseBackMaterial.IsChecked.HasValue ? cbUseBackMaterial.IsChecked.Value : false;
                    GameEngineController.ReplaceModelAndNotify(layeredModel, definition);
                }
            }
        }

        private void btTexture_Click(object sender, RoutedEventArgs e)
        {
            EditTexture();
        }

        private void EditTexture()
        {
            var layeredModel = State.Editor.ActionSelect.SelectedModel;

            if (layeredModel != null)
            {
                var definition = GameEngineController.GetDefinition<WallDefinition>(layeredModel);

                var dialog = new TextureEditorWindow
                {
                    Owner = Window.GetWindow(this),
                    Texture = definition.Metadata.Texture,
                    ShowInTaskbar = false,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                };

                dialog.OnTextureChanged += (sender, e)
                    => OnChangeTexture<WallDefinition>(def => def.Metadata.Texture = e);

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

        private void menuEdit_Click(object sender, RoutedEventArgs e)
        {
            EditTexture();
        }

        private void menuRemove_Click(object sender, RoutedEventArgs e)
        {
            var layeredModel = State.Editor.ActionSelect.SelectedModel;

            if (layeredModel != null)
            {
                var definition = GameEngineController.GetDefinition<WallDefinition>(layeredModel);
                definition.Metadata.Texture = null;
                GameEngineController.ReplaceModelAndNotify(layeredModel, definition);
            }
        }
    }
}
