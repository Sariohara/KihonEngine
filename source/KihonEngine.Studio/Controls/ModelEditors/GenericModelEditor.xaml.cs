using KihonEngine.GameEngine;
using KihonEngine.GameEngine.Graphics.ModelDefinitions;
using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using KihonEngine.GameEngine.State;
using KihonEngine.Services;
using System;
using System.Linq;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace KihonEngine.Studio.Controls
{
    /// <summary>
    /// Interaction logic for GenericModel.xaml
    /// </summary>
    public partial class GenericModelEditor : UserControl, ISynchronizedIO
    {
        private ILogService LogService
            => Container.Get<ILogService>();

        private Timer timer;
        private int timerIntervalInMilliseconds = 500;
        private bool processing;

        private IGameEngineState _lastState;
        private bool _stateUpdated;

        public GenericModelEditor()
        {
            InitializeComponent();

            timer = new Timer();
            timer.Interval = timerIntervalInMilliseconds;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        public void Synchronize(IGameEngineState state)
        {
            // Defered due to high refresh rate
            _lastState = state;
            _stateUpdated = true;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!processing)
            {
                try
                {
                    processing = true;

                    if (_lastState != null && _stateUpdated)
                    {
                        Dispatcher.Invoke(() => SynchronizeInternal(_lastState));
                        _stateUpdated = false;
                    }
                }
                catch (Exception ex)
                {
                    LogService.Log($"ERROR:{nameof(GenericModelEditor)}:{ex.Message}");
                }
                finally
                {
                    processing = false;
                }
            }
        }

        private void SynchronizeInternal(IGameEngineState state)
        {
            propertyGrid.IsEnabled = state.EngineMode == EngineMode.EditorMode && state.Editor.ActionSelect.SelectedModel != null;

            if (propertyGrid.IsEnabled)
            {
                tbDetails.Text = GetDetails(state.Editor.ActionSelect.SelectedModel);
            }
            else
            {
                tbDetails.Text = string.Empty;
            }
        }

        private string GetDetails(LayeredModel3D layeredModel3D)
        {
            var textInfos = $"{layeredModel3D.Type} - {layeredModel3D.Children.Count} Mesh(es)\r\n";

            if (layeredModel3D.Metadata.ContainsKey(layeredModel3D.Type.ToString()))
            {
                textInfos += $"Metadata\r\n";
                var layerMetadata = layeredModel3D.Metadata[layeredModel3D.Type.ToString()];
                if (layeredModel3D.Type == ModelType.Ceiling)
                {
                    var metadata = layerMetadata as CeilingMetadata;
                    textInfos += $"    Length:{metadata.ZSize}\r\n";
                    textInfos += $"    Width:{metadata.XSize}\r\n";
                    textInfos += $"    UseBackMaterial:{metadata.UseBackMaterial}\r\n";
                }
                else if (layeredModel3D.Type == ModelType.Floor)
                {
                    var metadata = layerMetadata as FloorMetadata;
                    textInfos += $"    Texture:{metadata.Texture}\r\n";
                    textInfos += $"    Length:{metadata.ZSize}\r\n";
                    textInfos += $"    Width:{metadata.XSize}\r\n";
                    textInfos += $"    UseBackMaterial:{metadata.UseBackMaterial}\r\n";
                }
                else if (layeredModel3D.Type == ModelType.Light)
                {
                    var metadata = layerMetadata as LightMetadata;
                    textInfos += $"    Direction:{metadata.Direction}\r\n";
                }
                else if (layeredModel3D.Type == ModelType.Skybox)
                {
                    var metadata = layerMetadata as SkyboxMetadata;
                    textInfos += $"    Name:{metadata.Name}\r\n";
                    textInfos += $"    Width:{metadata.Size}\r\n";
                    textInfos += $"    Normal:({metadata.Normal.X}, {metadata.Normal.Y}, {metadata.Normal.Z})\r\n";
                    textInfos += $"    UseBackMaterial:{metadata.UseBackMaterial}\r\n";
                }
                else if (layeredModel3D.Type == ModelType.Volume)
                {
                    var metadata = layerMetadata as VolumeMetadata;
                    textInfos += $"    Length:{metadata.ZSize}\r\n";
                    textInfos += $"    Width:{metadata.XSize}\r\n";
                    textInfos += $"    Height:{metadata.YSize}\r\n";
                    textInfos += $"    UseBackMaterial:{metadata.UseBackMaterial}\r\n";
                }
                else if (layeredModel3D.Type == ModelType.Wall)
                {
                    var metadata = layerMetadata as WallMetadata;
                    textInfos += $"    Width:{metadata.XSize}\r\n";
                    textInfos += $"    Height:{metadata.YSize}\r\n";
                    textInfos += $"    UseBackMaterial:{metadata.UseBackMaterial}\r\n";
                }
                else
                {
                    textInfos += $"    <{layeredModel3D.Metadata[layeredModel3D.Type.ToString()]}>\r\n";
                }
            }

            if (layeredModel3D.Type != ModelType.Light)
            {
                var translationMatrix = layeredModel3D.Translation.Value;
                textInfos += $"Position:({translationMatrix.OffsetX}, {translationMatrix.OffsetY}, {translationMatrix.OffsetZ})\r\n";
                textInfos += $"AxisXRotationAngle:{layeredModel3D.AxisXRotationAngle}\r\n";
                textInfos += $"AxisYRotationAngle:{layeredModel3D.AxisYRotationAngle}\r\n";
                textInfos += $"AxisZRotationAngle:{layeredModel3D.AxisZRotationAngle}\r\n";
                textInfos += $"Color:{layeredModel3D.GetColorFromMetadata()}\r\n";
            }

            Color selectedColor = Colors.Transparent;
            foreach (var children in layeredModel3D.Children)
            {

                var childrens = ((Model3DGroup)children).Children;
                foreach (var child in childrens)
                {
                    if (child is DirectionalLight)
                    {
                        var model = (DirectionalLight)child;
                        textInfos += $"DirectionalLight:\r\n";
                        textInfos += $"    Color:{model.Color}\r\n";
                        textInfos += $"    Direction:({model.Direction.X}, {model.Direction.Y}, {model.Direction.Z})\r\n";
                    }
                    else if (child is GeometryModel3D)
                    {
                        var model = (GeometryModel3D)child;
                        textInfos += "Mesh:\r\n";
                        textInfos += "    Material:" + model.Material.GetType().Name + "\r\n";

                        if (model.Material is MaterialGroup)
                        {
                            var materialGroup = (MaterialGroup)model.Material;
                            foreach (var material in materialGroup.Children)
                            {
                                textInfos += "        Material:" + material.GetType().Name + "\r\n";
                                if (material is DiffuseMaterial)
                                {
                                    var brush = ((DiffuseMaterial)material).Brush;
                                    if (brush is SolidColorBrush)
                                    {
                                        selectedColor = ((SolidColorBrush)brush).Color;
                                        textInfos += $"            {brush.GetType().Name}:{selectedColor}\r\n";
                                    }
                                    else if (brush is ImageBrush)
                                    {
                                        var imageSource = ((ImageBrush)brush).ImageSource;
                                        if (imageSource is BitmapImage)
                                        {
                                            var baseUri = ((BitmapImage)imageSource).BaseUri;
                                            textInfos += $"            {brush.GetType().Name}:{imageSource}\r\n";
                                        }
                                    }
                                }
                            }
                        }

                        MeshGeometry3D mesh = model.Geometry as MeshGeometry3D;
                        if (mesh.TextureCoordinates.Count > 0)
                        {
                            textInfos += "    TextureCoordinates:\r\n";
                            textInfos += "        (" + mesh.TextureCoordinates[0].ToString() + "), ";
                            textInfos += "(" + mesh.TextureCoordinates[1].ToString() + "), ";
                            textInfos += "(" + mesh.TextureCoordinates[2].ToString() + ")\r\n";
                        }

                        textInfos += "    Vertices:\r\n";
                        textInfos += "        (" + mesh.Positions[mesh.TriangleIndices[0]].ToString() + "), ";
                        textInfos += "(" + mesh.Positions[mesh.TriangleIndices[1]].ToString() + "), ";
                        textInfos += "(" + mesh.Positions[mesh.TriangleIndices[2]].ToString() + ")\r\n";
                    }
                }
            }

            return textInfos;
        }
    }
}
