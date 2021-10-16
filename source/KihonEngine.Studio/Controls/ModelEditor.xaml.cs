using KihonEngine.GameEngine;
using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using KihonEngine.GameEngine.State;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using KihonEngine.Studio.Controls.ModelEditors;
using KihonEngine.Services;
using KihonEngine.GameEngine.Graphics;
using KihonEngine.Studio.Helpers;
using System.Windows.Media.Imaging;
using System;
using KihonEngine.GameEngine.Graphics.ModelDefinitions;

namespace KihonEngine.Studio.Controls
{
    /// <summary>
    /// Interaction logic for PropertyWindows.xaml
    /// </summary>
    public partial class ModelEditor : UserControl, ISynchronizedIO
    {
        private ILogService LogService
            => Container.Get<ILogService>();
        private IGameEngineController GameEngineController
            => Container.Get<IGameEngineController>();
        private IGameEngineState State
            => Container.Get<IGameEngineState>();
        private IWorldEngine WorldEngine
            => Container.Get<IWorldEngine>();

        private int _transparentColorIndex;
        public ModelEditor()
        {
            InitializeComponent();

            var colorNames = typeof(Colors).GetProperties().Select(x => x.Name).ToArray();
            _transparentColorIndex = colorNames.ToList().IndexOf(nameof(Colors.Transparent));
            cbColors.ItemsSource = colorNames;
            cbColors.SelectedIndex = _transparentColorIndex;
        }

        private bool synchronizing;
        private LayeredModel3D _selectedModel;
        private ISynchronizedIO _customPropertyEditor;
        public void Synchronize(IGameEngineState state)
        {
            synchronizing = true;
            var selectionChanged = _selectedModel != state.Editor.ActionSelect.SelectedModel;
            _selectedModel = state.Editor.ActionSelect.SelectedModel;
            propertyGrid.IsEnabled = state.EngineMode == EngineMode.EditorMode && state.Editor.ActionSelect.SelectedModel != null;

            if (propertyGrid.IsEnabled)
            {
                panelHelp.Visibility = Visibility.Hidden;
                gridPositionAndRotation.Visibility = Visibility.Visible;
                gridGenericProperties.Visibility = Visibility.Visible;
                panelActions.Visibility = Visibility.Visible;

                if (_selectedModel.Type == ModelType.Light)
                {
                    DisablePositionAndRotationGrids();
                }
                else
                {
                    EnablePositionAndRotationGrids();

                    var position = _selectedModel.GetPosition();
                    tbPositionX.Text = position.X.ToString();
                    tbPositionY.Text = position.Y.ToString();
                    tbPositionZ.Text = position.Z.ToString();

                    tbRotationX.Text = _selectedModel.AxisXRotationAngle.ToString();
                    tbRotationY.Text = _selectedModel.AxisYRotationAngle.ToString();
                    tbRotationZ.Text = _selectedModel.AxisZRotationAngle.ToString();
                }

                TrySelectColor(state.Editor.ActionSelect.SelectedModel.GetColor());

                if (selectionChanged)
                {
                    panelCustomProperties.Children.Clear();

                    var index = state.Graphics.Level.IndexOf(_selectedModel);
                    lblModelTitle.Content = $"Model #{index} ({_selectedModel.Type})";
                    lblCustomPropertiesTitle.Content = $"{_selectedModel.Type} Properties";
                    if (_selectedModel.Type != ModelType.Group)
                    {
                        var assemblyName = this.GetType().Assembly.GetName().Name;
                        var sourceUri = $"pack://application:,,,/{assemblyName};component/Content/Images/Icons/icon-{_selectedModel.Type.ToString().ToLower()}-transparent.png";
                        imgModelType.Source = new BitmapImage(new Uri(sourceUri));
                    }
                    else
                    {
                        imgModelType.Source = null;
                    }

                    UserControl control = null;
                    if (_selectedModel.Type == ModelType.Ceiling)
                    {
                        control = new CeilingModelEditor();
                    }
                    else if (_selectedModel.Type == ModelType.Floor)
                    {
                        control = new FloorModelEditor();
                    }
                    else if (_selectedModel.Type == ModelType.Light)
                    {
                        control = new LightModelEditor();
                    }
                    else if (_selectedModel.Type == ModelType.Skybox)
                    {
                        control = new SkyboxModelEditor();
                    }
                    else if (_selectedModel.Type == ModelType.Volume)
                    {
                        control = new VolumeModelEditor();
                    }
                    else if (_selectedModel.Type == ModelType.Wall)
                    {
                        control = new WallModelEditor();
                    }
                    else
                    {
                        control = new GenericModelEditor();
                    }

                    panelCustomProperties.Children.Add(control);
                    _customPropertyEditor = (ISynchronizedIO)control;
                }

                _customPropertyEditor.Synchronize(state);
            }
            else
            {
                DisablePositionAndRotationGrids();

                cbColors.SelectedIndex = _transparentColorIndex;
                lblModelTitle.Content = string.Empty;
                panelHelp.Visibility = Visibility.Visible;
                gridPositionAndRotation.Visibility = Visibility.Hidden;
                gridGenericProperties.Visibility = Visibility.Hidden;
                panelActions.Visibility = Visibility.Hidden;
                imgModelType.Source = null;
                lblCustomPropertiesTitle.Content = string.Empty;
                panelCustomProperties.Children.Clear();
            }

            synchronizing = false;
        }

        private void EnablePositionAndRotationGrids()
        {
            gridPositionAndRotation.IsEnabled = true;
        }

        private void DisablePositionAndRotationGrids()
        {
            gridPositionAndRotation.IsEnabled = false;

            tbPositionX.Text = string.Empty;
            tbPositionY.Text = string.Empty;
            tbPositionZ.Text = string.Empty;

            tbRotationX.Text = string.Empty;
            tbRotationY.Text = string.Empty;
            tbRotationZ.Text = string.Empty;
        }

        private bool _disableColorChange;
        private void cbColors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!synchronizing && !_disableColorChange && this.IsLoaded)
            {
                var propertyName = cbColors.SelectedItem.ToString();
                PropertyInfo property = typeof(Colors).GetProperty(propertyName);
                var color = (System.Windows.Media.Color)property.GetValue(null, null);

                var definitionBuilder = new ModelDefinitionBuilder();
                var definition = definitionBuilder.CreateModelDefinition(State.Editor.ActionSelect.SelectedModel);
                definition.Color = color;
                State.Editor.CurrentColor = color;
                //State.Editor.ActionSelect.SelectedModel.SetColor(color);
                GameEngineController.ReplaceModelAndNotify(State.Editor.ActionSelect.SelectedModel, definition);
            }
        }

        private void TrySelectColor(Color target)
        {
            PropertyInfo[] properties = typeof(Colors).GetProperties();
            int i = 0;
            foreach (PropertyInfo property in properties)
            {
                var color = property.GetValue(null, null);

                if (((System.Windows.Media.Color)color).A == target.A &&
                       ((System.Windows.Media.Color)color).R == target.R &&
                       ((System.Windows.Media.Color)color).G == target.G &&
                       ((System.Windows.Media.Color)color).B == target.B)
                {
                    _disableColorChange = true;
                    cbColors.SelectedIndex = i;
                    _disableColorChange = false;
                    break;
                }

                i++;
            }
        }

        private void tbPositionX_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            var layeredModel = State.Editor.ActionSelect.SelectedModel;
            if (layeredModel != null)
            {
                var position = layeredModel.GetPosition();

                InputHelper.TryUpdate(
                    tbPositionX.Text, 
                    step => layeredModel.Translate(new Vector3D(step, position.Y, position.Z)));
                
                GameEngineController.NotifyIOs();
            }
        }

        private void tbPositionY_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            var layeredModel = State.Editor.ActionSelect.SelectedModel;
            if (layeredModel != null)
            {
                var position = layeredModel.GetPosition();

                InputHelper.TryUpdate(
                    tbPositionY.Text,
                    step => layeredModel.Translate(new Vector3D(position.X, step, position.Z)));

                GameEngineController.NotifyIOs();
            }
        }

        private void tbPositionZ_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            var layeredModel = State.Editor.ActionSelect.SelectedModel;
            if (layeredModel != null)
            {
                var position = layeredModel.GetPosition();

                InputHelper.TryUpdate(
                    tbPositionZ.Text,
                    step => layeredModel.Translate(new Vector3D(position.X, position.Y, step)));

                GameEngineController.NotifyIOs();
            }
        }
        
        private void tbRotationX_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            var layeredModel = State.Editor.ActionSelect.SelectedModel;
            if (layeredModel != null)
            {
                InputHelper.TryUpdate(
                    tbRotationX.Text,
                    angle => layeredModel.RotateByAxisX(angle));

                GameEngineController.NotifyIOs();
            }
        }

        private void tbRotationY_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            var layeredModel = State.Editor.ActionSelect.SelectedModel;
            if (layeredModel != null)
            {
                InputHelper.TryUpdate(
                    tbRotationY.Text,
                    angle => layeredModel.RotateByAxisY(angle));

                GameEngineController.NotifyIOs();
            }
        }

        private void tbRotationZ_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            var layeredModel = State.Editor.ActionSelect.SelectedModel;
            if (layeredModel != null)
            {
                InputHelper.TryUpdate(
                    tbRotationZ.Text,
                    angle => layeredModel.RotateByAxisZ(angle));

                GameEngineController.NotifyIOs();
            }
        }

        private void btnRemoveModel_Click(object sender, RoutedEventArgs e)
        {
            var layeredModel = State.Editor.ActionSelect.SelectedModel;
            if (layeredModel != null)
            {
                var result = MessageBox.Show(
                    $"Do you really want to remove this selected {layeredModel.Type}?",
                    "Remove selected model", 
                    MessageBoxButton.YesNo, 
                    MessageBoxImage.Warning, 
                    MessageBoxResult.No);

                if (result == MessageBoxResult.Yes)
                {
                    State.Graphics.Level.Remove(layeredModel);
                    State.Graphics.Viewport.Children.Remove(layeredModel.GetModel());
                    State.Editor.ActionSelect.SelectedModel = null;
                    GameEngineController.NotifyIOs();
                }
            }
        }
    }
}
