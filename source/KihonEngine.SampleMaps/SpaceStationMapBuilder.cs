using KihonEngine.GameEngine.Graphics.Maps;
using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace KihonEngine.SampleMaps
{
    public class SpaceStationMapBuilder : IMapBuilder
    {
        public string MapName => "Space Station";

        public Point3D PlayerPosition => new Point3D(10, 10, 70);

        public Vector3D PlayerLookDirection => new Vector3D(0, 0, -1);

        public List<LayeredModel3D> CreateMap()
        {
            var level = new List<LayeredModel3D>();

            var defaultColor = Colors.Transparent;
            var volumeBuilder = new VolumeBuilder(defaultColor, level);
            var wallBuilder = new WallBuilder(defaultColor, level);
            var ceilingBuilder = new CeilingBuilder(defaultColor, level);
            var floorBuilder = new FloorBuilder(defaultColor, level);
            var lightBuilder = new LightBuilder(Colors.Transparent, level);
            var skyboxBuilder = new SkyboxBuilder(Colors.Transparent, level);

            // Lights
            lightBuilder.Create(new Vector3D(-3, -4, -5));
            lightBuilder.Create(new Vector3D(3, 4, 5));

            // Sky box
            skyboxBuilder.Create(-5000, -5000, -5000, 10000, new Vector3D(-3, -4, -5), "sky0");

            // Floor : central
            var midFloorSize = 100;
            floorBuilder.Color = Colors.LightGray;
            var model = floorBuilder.Create(-100, 0, -midFloorSize, midFloorSize * 2);
            floorBuilder.ApplyTexture(model, "floor0.jpg", TileMode.Tile, Stretch.Uniform, 0.05, 0.05);

            return level;
        }
    }
}
