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
                    tbPositionStep.Text = state.Editor.TranslationStep.ToString();

                    tbRotationX.Text = _selectedModel.AxisXRotationAngle.ToString();
                    tbRotationY.Text = _selectedModel.AxisYRotationAngle.ToString();
                    tbRotationZ.Text = _selectedModel.AxisZRotationAngle.ToString();
                    tbRotationStep.Text = state.Editor.RotationStep.ToString();
                }

                TrySelectColor(state.Editor.ActionSelect.SelectedModel.GetColor());

                if (selectionChanged)
                {
                    panelCustomProperties.Children.Clear();

                    var index = state.Graphics.Level.IndexOf(_selectedModel);
                    lblModelType.Content = $"#{index} ({_selectedModel.Type})";

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
                lblModelType.Content = string.Empty;
                panelCustomProperties.Children.Clear();
            }

            synchronizing = false;
        }

        private void EnablePositionAndRotationGrids()
        {
            gridPosition.IsEnabled = true;
            gridRotation.IsEnabled = true;
        }

        private void DisablePositionAndRotationGrids()
        {
            gridPosition.IsEnabled = false;
            gridRotation.IsEnabled = false;

            tbPositionX.Text = string.Empty;
            tbPositionY.Text = string.Empty;
            tbPositionZ.Text = string.Empty;
            tbPositionStep.Text = string.Empty;

            tbRotationX.Text = string.Empty;
            tbRotationY.Text = string.Empty;
            tbRotationZ.Text = string.Empty;
            tbRotationStep.Text = string.Empty;
        }

        private bool _disableColorChange;
        private void cbColors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!synchronizing && !_disableColorChange && this.IsLoaded)
            {
                var propertyName = cbColors.SelectedItem.ToString();
                PropertyInfo property = typeof(Colors).GetProperty(propertyName);
                var color = (System.Windows.Media.Color)property.GetValue(null, null);

                State.Editor.CurrentColor = color;
                State.Editor.ActionSelect.SelectedModel.SetColor(color);
                GameEngineController.NotifyIOs();
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

        private void btnPositionXPlus_Click(object sender, RoutedEventArgs e)
        {
            var layeredModel = State.Editor.ActionSelect.SelectedModel;
            if (layeredModel != null)
            {
                var step = State.Editor.TranslationStep;
                layeredModel.TranslateOnAxisX(step);
                GameEngineController.NotifyIOs();
            }
        }

        private void btnPositionYPlus_Click(object sender, RoutedEventArgs e)
        {
            var layeredModel = State.Editor.ActionSelect.SelectedModel;
            if (layeredModel != null)
            {
                var step = State.Editor.TranslationStep;
                layeredModel.TranslateOnAxisY(step);
                GameEngineController.NotifyIOs();
            }
        }

        private void btnPositionZPlus_Click(object sender, RoutedEventArgs e)
        {
            var layeredModel = State.Editor.ActionSelect.SelectedModel;
            if (layeredModel != null)
            {
                var step = State.Editor.TranslationStep;
                layeredModel.TranslateOnAxisZ(step);
                GameEngineController.NotifyIOs();
            }
        }

        private void btnPositionXMinus_Click(object sender, RoutedEventArgs e)
        {
            var layeredModel = State.Editor.ActionSelect.SelectedModel;
            if (layeredModel != null)
            {
                var step = State.Editor.TranslationStep;
                layeredModel.TranslateOnAxisX(-step);
                GameEngineController.NotifyIOs();
            }
        }

        private void btnPositionYMinus_Click(object sender, RoutedEventArgs e)
        {
            var layeredModel = State.Editor.ActionSelect.SelectedModel;
            if (layeredModel != null)
            {
                var step = State.Editor.TranslationStep;
                layeredModel.TranslateOnAxisY(-step);
                GameEngineController.NotifyIOs();
            }
        }

        private void btnPositionZMinus_Click(object sender, RoutedEventArgs e)
        {
            var layeredModel = State.Editor.ActionSelect.SelectedModel;
            if (layeredModel != null)
            {
                var step = State.Editor.TranslationStep;
                layeredModel.TranslateOnAxisZ(-step);
                GameEngineController.NotifyIOs();
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

        private void tbPositionStep_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            InputHelper.TryUpdate(
                tbPositionStep.Text,
                step => State.Editor.TranslationStep = step);

            GameEngineController.NotifyIOs();
        }

        private void btnRotationXPlus_Click(object sender, RoutedEventArgs e)
        {
            var layeredModel = State.Editor.ActionSelect.SelectedModel;
            if (layeredModel != null)
            {
                layeredModel.RotateByAxisX(layeredModel.AxisXRotationAngle + State.Editor.RotationStep);
                GameEngineController.NotifyIOs();
            }
        }

        private void btnRotationYPlus_Click(object sender, RoutedEventArgs e)
        {
            var layeredModel = State.Editor.ActionSelect.SelectedModel;
            if (layeredModel != null)
            {
                layeredModel.RotateByAxisY(layeredModel.AxisYRotationAngle + State.Editor.RotationStep);
                GameEngineController.NotifyIOs();
            }
        }

        private void btnRotationZPlus_Click(object sender, RoutedEventArgs e)
        {
            var layeredModel = State.Editor.ActionSelect.SelectedModel;
            if (layeredModel != null)
            {
                layeredModel.RotateByAxisZ(layeredModel.AxisZRotationAngle + State.Editor.RotationStep);
                GameEngineController.NotifyIOs();
            }
        }

        private void btnRotationXMinus_Click(object sender, RoutedEventArgs e)
        {
            var layeredModel = State.Editor.ActionSelect.SelectedModel;
            if (layeredModel != null)
            {
                layeredModel.RotateByAxisX(layeredModel.AxisXRotationAngle - State.Editor.RotationStep);
                GameEngineController.NotifyIOs();
            }
        }

        private void btnRotationYMinus_Click(object sender, RoutedEventArgs e)
        {
            var layeredModel = State.Editor.ActionSelect.SelectedModel;
            if (layeredModel != null)
            {
                layeredModel.RotateByAxisY(layeredModel.AxisYRotationAngle - State.Editor.RotationStep);
                GameEngineController.NotifyIOs();
            }
        }

        private void btnRotationZMinus_Click(object sender, RoutedEventArgs e)
        {
            var layeredModel = State.Editor.ActionSelect.SelectedModel;
            if (layeredModel != null)
            {
                layeredModel.RotateByAxisZ(layeredModel.AxisZRotationAngle - State.Editor.RotationStep);
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

        private void tbRotationStep_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            InputHelper.TryUpdate(
                tbRotationStep.Text,
                step => State.Editor.RotationStep = step);

            GameEngineController.NotifyIOs();
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
