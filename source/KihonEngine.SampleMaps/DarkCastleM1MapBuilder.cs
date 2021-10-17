using KihonEngine.GameEngine.Graphics.Maps;
using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace KihonEngine.SampleMaps
{
    public class DarkCastleM1MapBuilder : IMapBuilder
    {
        public string MapName => "Dark Castle M1";

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
            skyboxBuilder.Create(-5000, -5000, -5000, 10000, new Vector3D(-3, -4, -5), "sky0-full.png");

            // Bridge
            volumeBuilder.Color = Colors.LightGray;
            volumeBuilder.Create(50, -200, -425, 25, 300, 25);
            volumeBuilder.Create(50, 125, -425, 25, 50, 25);
            volumeBuilder.Create(150, 50, -350, 25, 50, 25);

            volumeBuilder.Create(-75, -200, -375, 25, 275, 25);
            volumeBuilder.Create(-70, 80, -375, 25, 25, 25);

            volumeBuilder.Create(50, -200, -375, 25, 220, 25);
            volumeBuilder.Create(-75, -200, -300, 25, 220, 25);
            volumeBuilder.Create(50, -200, -300, 25, 220, 25);
            volumeBuilder.Create(-75, -200, -200, 25, 200, 50);
            volumeBuilder.Create(-25, -200, -300, 50, 175, 150);

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
            volumeBuilder.Create(-75, -200, -125, 25, 195, 25);

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
            volumeBuilder.Create(-400, 50, -50, 25, 50, 25);
            volumeBuilder.Create(-300, -200, -50, 25, 225, 25);
            volumeBuilder.Color = Colors.SandyBrown;
            volumeBuilder.Create(-300, -200, 0, 200, 175, 25);
            volumeBuilder.Create(-125, -200, -75, 25, 195, 175);
            volumeBuilder.Create(-150, -200, 25, 50, 200, 25);
            CreateTree(volumeBuilder, -105, -25, 20, 10);
            CreateTree(volumeBuilder, -105, -50, 40, 10);
            CreateTree(volumeBuilder, -105, -70, 30, 10);
            CreateTree(volumeBuilder, -75, -75, 30, 10);

            // Floor : right part
            volumeBuilder.Color = Colors.LightGray;
            volumeBuilder.Create(90, -200, -110, 25, 215, 25);
            volumeBuilder.Create(50, -200, -115, 25, 205, 25);
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
            volumeBuilder.Create(-25, 0, 115, 5, 40, 10);
            volumeBuilder.Create(20, 0, 115, 5, 40, 10);
            volumeBuilder.Create(-25, 40, 115, 50, 10, 10);
            volumeBuilder.Color = Colors.DarkRed;
            volumeBuilder.Create(-20, 0, 120, 19, 40, 5);
            volumeBuilder.Create(1, 0, 120, 19, 40, 5);

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


            volumeBuilder.Create(-100, 0, -100, 5, 5, 225);
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

        private void CreateTree(VolumeBuilder cubeBuilder, int x, int z, int ySize, int leafSize)
        {
            cubeBuilder.Color = Colors.Brown;
            cubeBuilder.Create(x, 0, z, 2, ySize, 2);
            cubeBuilder.Color = Colors.Green;
            cubeBuilder.Create(x - 5, ySize, z - 5, leafSize, leafSize, leafSize);
        }
    }
}
