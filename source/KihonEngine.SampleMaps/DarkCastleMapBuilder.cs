using KihonEngine.GameEngine.Graphics.Maps;
using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace KihonEngine.SampleMaps
{
    public class DarkCastleMapBuilder : IMapBuilder
    {
        public string MapName => "Dark Castle";

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

            // Bridge
            volumeBuilder.Color = Colors.LightGray;
            volumeBuilder.Create(50, -200, -425, 25, 25, 300);
            volumeBuilder.Create(50, 125, -425, 25, 25, 50);
            volumeBuilder.Create(150, 50, -350, 25, 25, 50);

            volumeBuilder.Create(-75, -200, -375, 25, 25, 275);
            volumeBuilder.Create(-70, 80, -375, 25, 25, 25);

            volumeBuilder.Create(50, -200, -375, 25, 25, 220);
            volumeBuilder.Create(-75, -200, -300, 25, 25, 220);
            volumeBuilder.Create(50, -200, -300, 25, 25, 220);
            volumeBuilder.Create(-75, -200, -200, 25, 50, 200);
            volumeBuilder.Create(-25, -200, -300, 50, 150, 175);

            // Floor : central
            floorBuilder.Color = Colors.SandyBrown;
            floorBuilder.Create(-25, 0, -100, 50, 25);
            floorBuilder.Color = Colors.SandyBrown;
            floorBuilder.Create(-25, 0, -75, 50);
            floorBuilder.Color = Colors.SandyBrown;
            floorBuilder.Create(-25, 0, -25, 50);
            floorBuilder.Color = Colors.Green;
            floorBuilder.Create(-25, 0, 25, 50);

            // Floor : left part
            volumeBuilder.Color = Colors.LightGray;
            volumeBuilder.Create(-75, -200, -125, 25, 25, 195);

            floorBuilder.Color = Colors.SandyBrown;
            floorBuilder.Create(-75, 0, -100, 50, 25);
            floorBuilder.Color = Colors.Green;
            floorBuilder.Create(-75, 0, -75, 50, 150);
            floorBuilder.Color = Colors.LightGray;
            floorBuilder.Create(-100, 0, -100, 25, 200);
            wallBuilder.Color = Colors.LightGray;
            var wall = wallBuilder.Create(-100, -200, -100, 200, 200);
            wall.RotateByAxisY(180);

            volumeBuilder.Color = Colors.LightGray;
            volumeBuilder.Create(-400, 50, -50, 25, 25, 50);
            volumeBuilder.Create(-300, -200, -50, 25, 25, 225);
            volumeBuilder.Color = Colors.SandyBrown;
            volumeBuilder.Create(-300, -200, 0, 200, 25, 175);
            volumeBuilder.Create(-125, -200, -75, 25, 175, 195);
            volumeBuilder.Create(-150, -200, 25, 50, 25, 200);
            CreateTree(volumeBuilder, -105, -25, 20, 10);
            CreateTree(volumeBuilder, -105, -50, 40, 10);
            CreateTree(volumeBuilder, -105, -70, 30, 10);
            CreateTree(volumeBuilder, -75, -75, 30, 10);

            // Floor : right part
            volumeBuilder.Color = Colors.LightGray;
            volumeBuilder.Create(90, -200, -110, 25, 25, 215);
            volumeBuilder.Create(50, -200, -115, 25, 25, 205);
            floorBuilder.Color = Colors.LightGray;
            floorBuilder.Create(25, 0, -100, 50, 25);
            floorBuilder.Color = Colors.Green;
            floorBuilder.Create(25, 0, -75, 50, 150);
            floorBuilder.Create(75, 0, -100, 25, 200);

            CreateTree(volumeBuilder, 75, -85, 20, 10);
            CreateTree(volumeBuilder, 75, -65, 20, 10);
            CreateTree(volumeBuilder, 65, -45, 10, 10);
            CreateTree(volumeBuilder, 75, -35, 20, 10);
            CreateTree(volumeBuilder, 85, -25, 30, 10);
            CreateTree(volumeBuilder, 85, -15, 10, 10);

            // Castle : Entry
            floorBuilder.Color = Colors.LightGray;
            floorBuilder.Create(-100, 0, 75, 175, 50);
            floorBuilder.Create(75, 0, 100, 25, 25);

            volumeBuilder.Color = Colors.DimGray;
            volumeBuilder.Create(-25, 0, 115, 5, 10, 40);
            volumeBuilder.Create(20, 0, 115, 5, 10, 40);
            volumeBuilder.Create(-25, 40, 115, 50, 10, 10);
            volumeBuilder.Color = Colors.DarkRed;
            volumeBuilder.Create(-20, 0, 120, 19, 5, 40);
            volumeBuilder.Create(1, 0, 120, 19, 5, 40);

            // Left part : battlements
            volumeBuilder.Color = Colors.DimGray;
            volumeBuilder.Create(-100, 0, -100, 75, 5, 5);
            volumeBuilder.Create(-90, 5, -100, 5, 5, 5);
            volumeBuilder.Create(-80, 5, -100, 5, 5, 5);
            volumeBuilder.Create(-70, 5, -100, 5, 5, 5);
            volumeBuilder.Create(-60, 5, -100, 5, 5, 5);
            volumeBuilder.Create(-50, 5, -100, 5, 5, 5);
            volumeBuilder.Create(-40, 5, -100, 5, 5, 5);
            volumeBuilder.Create(-30, 5, -100, 5, 5, 5);


            volumeBuilder.Create(-100, 0, -100, 5, 225, 5);
            volumeBuilder.Create(-100, 5, -100, 5, 5, 5);
            volumeBuilder.Create(-100, 5, -90, 5, 5, 5);
            volumeBuilder.Create(-100, 5, -80, 5, 5, 5);
            volumeBuilder.Create(-100, 5, -70, 5, 5, 5);
            volumeBuilder.Create(-100, 5, -60, 5, 5, 5);
            volumeBuilder.Create(-100, 5, -50, 5, 5, 5);
            volumeBuilder.Create(-100, 5, -40, 5, 5, 5);
            volumeBuilder.Create(-100, 5, -30, 5, 5, 5);
            volumeBuilder.Create(-100, 5, -20, 5, 5, 5);
            volumeBuilder.Create(-100, 5, -10, 5, 5, 5);
            volumeBuilder.Create(-100, 5, 0, 5, 5, 5);
            volumeBuilder.Create(-100, 5, 10, 5, 5, 5);
            volumeBuilder.Create(-100, 5, 20, 5, 5, 5);
            volumeBuilder.Create(-100, 5, 30, 5, 5, 5);
            volumeBuilder.Create(-100, 5, 40, 5, 5, 5);
            volumeBuilder.Create(-100, 5, 50, 5, 5, 5);
            volumeBuilder.Create(-100, 5, 60, 5, 5, 5);
            volumeBuilder.Create(-100, 5, 70, 5, 5, 5);
            volumeBuilder.Create(-100, 5, 80, 5, 5, 5);
            volumeBuilder.Create(-100, 5, 90, 5, 5, 5);
            volumeBuilder.Create(-100, 5, 100, 5, 5, 5);
            volumeBuilder.Create(-100, 5, 110, 5, 5, 5);
            volumeBuilder.Create(-100, 5, 120, 5, 5, 5);

            return level;
        }

        private void CreateTree(VolumeBuilder cubeBuilder, int x, int z, int h, int w)
        {
            cubeBuilder.Color = Colors.Brown;
            cubeBuilder.Create(x, 0, z, 2, 2, h);
            cubeBuilder.Color = Colors.Green;
            cubeBuilder.Create(x - 5, h, z - 5, w, w, w);
        }
    }
}
