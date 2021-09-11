using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.Graphics.Maps.Predefined
{
    public class LogoMapBuilder : IMapBuilder
    {
        public string MapName => PredefinedMapNames.Logo;

        public Point3D PlayerPosition => new Point3D(35, 10, 70);

        public Vector3D PlayerLookDirection => new Vector3D(0, 0, -1);

        public List<LayeredModel3D> CreateMap()
        {
            var level = new List<LayeredModel3D>();

            var defaultColor = Colors.Transparent;
            var volumeBuilder = new VolumeBuilder(defaultColor, level);
            var lightBuilder = new LightBuilder(Colors.Transparent, level);
            var skyboxBuilder = new SkyboxBuilder(Colors.Transparent, level);

            // Lights
            lightBuilder.Create(new Vector3D(-3, -4, -5));
            lightBuilder.Create(new Vector3D(3, 4, -5));

            // Sky box
            //skyboxBuilder.Color = Colors.DarkSlateGray;
            var color = new Color { R = 37, G = 37, B = 38 };
            skyboxBuilder.Color = color;
            
            skyboxBuilder.Create(-5000, -5000, -5000, 10000, new Vector3D(3, 4, 5));

            // Cubes
            var size = 30;

            var textureKi = CreateMaterial("ki.png");
            var textureHon = CreateMaterial("hon.png");

            var textureCoordinates1 = new[] { new Point(0, 1), new Point(1, 1), new Point(1, 0) };
            var textureCoordinates2 = new[] { new Point(0, 1), new Point(1, 0), new Point(0, 0) };

            volumeBuilder.Color = Colors.WhiteSmoke;
            var model = volumeBuilder.Create(0, 0, -50, size);
            model.RotateByAxisY(35);
            ApplyTextureToVolume(model, "Front1", textureKi, textureCoordinates1);
            ApplyTextureToVolume(model, "Front2", textureKi, textureCoordinates2);

            volumeBuilder.Color = Colors.WhiteSmoke;
            model = volumeBuilder.Create(50, 0, -50, size);
            model.RotateByAxisY(-35);
            ApplyTextureToVolume(model, "Front1", textureHon, textureCoordinates1);
            ApplyTextureToVolume(model, "Front2", textureHon, textureCoordinates2);

            volumeBuilder.Color = Colors.SeaGreen;
            volumeBuilder.Create(0, 0, -10, 10);
            volumeBuilder.Create(36, 0, -50, 10);
            volumeBuilder.Create(70, 0, -10, 10);

            return level;
        }

        private MaterialGroup CreateMaterial(string filename)
        {
            var materiaGroup = new MaterialGroup();
            var brush = new ImageBrush(new BitmapImage(new Uri(@$"GameEngine\Graphics\Images\Textures\{filename}", UriKind.Relative)));
            brush.TileMode = TileMode.Tile;
            brush.Stretch = Stretch.Uniform;
            brush.Viewport = new Rect(new Point(0, 0), new Point(1, 1));
            brush.ViewboxUnits = BrushMappingMode.RelativeToBoundingBox;
            materiaGroup.Children.Add(new DiffuseMaterial(brush));

            return materiaGroup;
        }

        private void ApplyTextureToVolume(LayeredModel3D layeredModel, string face, MaterialGroup material, Point[] textureCoordinates)
        {
            var model3DGroup = (Model3DGroup)layeredModel.Metadata[face];
            var geometry = (GeometryModel3D)model3DGroup.Children[0];
            geometry.Material = material;
            var mesh = (MeshGeometry3D)geometry.Geometry;

            foreach (var point in textureCoordinates)
            {
                mesh.TextureCoordinates.Add(point);
            }
        }
    }
}
