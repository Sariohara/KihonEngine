using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.Graphics.Maps.Predefined
{
    public class NewMapBuilder : IMapBuilder
    {
        public string MapName => PredefinedMapNames.New;

        public Point3D PlayerPosition => new Point3D(10, 10, 70);

        public Vector3D PlayerLookDirection => new Vector3D(0, 0, -1);

        public List<LayeredModel3D> CreateMap()
        {
            var level = new List<LayeredModel3D>();

            var defaultColor = Colors.Transparent;
            var volumeBuilder = new VolumeBuilder(defaultColor, level);
            var floorBuilder = new FloorBuilder(defaultColor, level);
            var lightBuilder = new LightBuilder(Colors.Transparent, level);
            var skyboxBuilder = new SkyboxBuilder(Colors.Transparent, level);

            // Lights
            lightBuilder.Create(new Vector3D(-3, -4, -5));
            lightBuilder.Create(new Vector3D(3, 4, 5));

            // Sky box
            skyboxBuilder.Create(-5000, -5000, -5000, 10000, new Vector3D(-3, -4, -5), "sky1-full.png");

            // Origin
            CreateOrigin(volumeBuilder);

            // Floor : central
            var midFloorSize = 100;
            floorBuilder.UseBackMaterial = true;
            floorBuilder.Color = Colors.LightGray;
            var model = floorBuilder.Create(-100, 0, -midFloorSize, midFloorSize * 2);
            floorBuilder.ApplyTexture(model, "default.png", TileMode.Tile, Stretch.Uniform, 0.05, 0.05);

            return level;
        }

        private void CreateOrigin(VolumeBuilder volumeBuilder)
        {
            volumeBuilder.Color = Colors.White;
            var model = volumeBuilder.Create(0, 0, 0, 0.5);
            model.Tags.Add("origin");
            model.Tags.Add("point");

            volumeBuilder.Color = Colors.LightGreen;
            model = volumeBuilder.Create(0, 0.5, 0, 0.5, 15, 0.5);
            model.Tags.Add("origin");
            model.Tags.Add("axisY");

            volumeBuilder.Color = Colors.Red;
            model = volumeBuilder.Create(0.5, 0, 0, 15, 0.5, 0.5);
            model.Tags.Add("origin");
            model.Tags.Add("axisX");

            volumeBuilder.Color = Colors.Blue;
            model = volumeBuilder.Create(0, 0, 0.5, 0.5, 0.5, 15);
            model.Tags.Add("origin");
            model.Tags.Add("axisZ");
        }
    }
}
