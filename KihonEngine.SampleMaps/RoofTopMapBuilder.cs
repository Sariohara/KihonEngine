using KihonEngine.GameEngine.Graphics.Maps;
using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace KihonEngine.SampleMaps
{
    public class RoofTopMapBuilder : IMapBuilder
    {
        public string MapName => "Roof Top";

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

            // Skybox

            skyboxBuilder.Color = Colors.AntiqueWhite;
            skyboxBuilder.Create(-5000, -5000, -5000, 10000, new Vector3D(-3, -4, -5));

            // Front Walls
            wallBuilder.UseBackMaterial = true;
            wallBuilder.Color = Colors.Cyan;
            var wall = wallBuilder.Create(-50, 0, -50, 10, 30);
            wall.RotateByAxisY(90);

            wallBuilder.Color = Colors.Cyan;
            wall = wallBuilder.Create(50, 0, -50, 10, 30);
            wall.RotateByAxisY(-90);

            wallBuilder.Color = Colors.LightCoral;
            wall = wallBuilder.Create(-50, 0, -65, 10, 30);
            wall.RotateByAxisY(45);

            wallBuilder.Color = Colors.LightCoral;
            wall = wallBuilder.Create(50, 0, -65, 10, 30);
            wall.RotateByAxisY(-45);

            wallBuilder.Color = Colors.MediumPurple;
            wallBuilder.Create(-50, 0, -80, 10, 30);

            wallBuilder.Color = Colors.Tomato;
            wallBuilder.Create(50, 0, -80, 10, 30);

            // Front table
            volumeBuilder.Color = Colors.LightGray;
            volumeBuilder.Create(-10, 0, -40, 10, 30, 7);

            volumeBuilder.Color = Colors.Red;
            volumeBuilder.Create(5, 0, -40);
            volumeBuilder.Create(10, 0, -40, 2, 5, 10);

            volumeBuilder.Create(-20, 0, -40);
            volumeBuilder.Create(-22, 0, -40, 2, 5, 10);

            volumeBuilder.Create(5, 0, -30);
            volumeBuilder.Create(10, 0, -30, 2, 5, 10);

            volumeBuilder.Create(-20, 0, -30);
            volumeBuilder.Create(-22, 0, -30, 2, 5, 10);

            volumeBuilder.Create(5, 0, -20);
            volumeBuilder.Create(10, 0, -20, 2, 5, 10);

            volumeBuilder.Create(-20, 0, -20);
            volumeBuilder.Create(-22, 0, -20, 2, 5, 10);


            volumeBuilder.Color = Colors.LightGray;
            // Cube : pillar 1
            volumeBuilder.Create(-25, 0, 15);
            volumeBuilder.Create(-25, 6, 15);
            volumeBuilder.Create(-25, 12, 15);
            volumeBuilder.Create(-25, 18, 15);

            // Cube : top wall
            volumeBuilder.Create(-25, 24, 15);
            volumeBuilder.Create(-19, 24, 15);
            volumeBuilder.Create(-13, 24, 15);
            volumeBuilder.Create(-7, 24, 15);
            volumeBuilder.Create(-1, 24, 15);
            volumeBuilder.Create(5, 24, 15);
            volumeBuilder.Create(11, 24, 15);

            // Cube : pillar 2
            volumeBuilder.Create(11, 0, 15);
            volumeBuilder.Create(11, 6, 15);
            volumeBuilder.Create(11, 12, 15);
            volumeBuilder.Create(11, 18, 15);
            volumeBuilder.Create(11, 24, 15);

            // Tree structures
            volumeBuilder.Color = Colors.Brown;
            volumeBuilder.Create(60, 0, 12);
            volumeBuilder.Create(60, 6, 12);
            volumeBuilder.Create(60, 12, 12);
            volumeBuilder.Color = Colors.Green;
            volumeBuilder.Create(60, 18, 12);
            
            volumeBuilder.Create(60, 18, 18);
            volumeBuilder.Create(60, 18, 6);
            volumeBuilder.Create(54, 18, 12);
            volumeBuilder.Create(66, 18, 12);

            volumeBuilder.Create(60, 24, 12);
            volumeBuilder.Create(60, 30, 12);
            volumeBuilder.Create(60, 24, 18);
            volumeBuilder.Create(60, 24, 6);
            volumeBuilder.Create(54, 24, 12);
            volumeBuilder.Create(66, 24, 12);

            // Umbrella
            ceilingBuilder.Color = Colors.Yellow;
            ceilingBuilder.UseBackMaterial = true;
            ceilingBuilder.Create(-25, 25, 20, 40, 60);

            volumeBuilder.Color = Colors.Brown;
            var uSize = 2;
            volumeBuilder.Create(-25, 0, 20, uSize, uSize, 25);
            volumeBuilder.Create(15- uSize, 0, 20, uSize, uSize, 25);
            volumeBuilder.Create(-25, 0, 80- uSize, uSize, uSize, 25);
            volumeBuilder.Create(15 - uSize, 0, 80 - uSize, uSize, uSize, 25);

            // Borders
            volumeBuilder.Color = Colors.LightGray;
            volumeBuilder.Create(-100, 0, -100, 200, 5, 5);
            volumeBuilder.Create(100 - 5, 0, -100, 5, 200, 5);
            volumeBuilder.Create(-100, 0, 100 - 5, 200, 5, 5);
            volumeBuilder.Create(-100, 0, -100, 5, 200, 5);


            volumeBuilder.Create(-100, 0, -100, 12);
            volumeBuilder.Color = Colors.Red;
            volumeBuilder.Create(100 - 12, 0, -100, 12);
            volumeBuilder.Color = Colors.Blue;
            volumeBuilder.Create(100 - 12, 0, 100 - 12, 12);
            volumeBuilder.Color = Colors.Yellow;
            volumeBuilder.Create(-100, 0, 100 - 12, 12);

            // Floor
            floorBuilder.Color = Colors.Green;
            floorBuilder.Create(-100, 0, -100, 200);

            return level;
        }
    }
}
