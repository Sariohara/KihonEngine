using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using System.Collections.Generic;
using System.Windows.Media;
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
            var color = new Color { R = 37, G = 37, B = 38 };
            skyboxBuilder.Color = Colors.DarkSlateGray;
            
            skyboxBuilder.Create(-5000, -5000, -5000, 10000, new Vector3D(3, 4, 5));

            // Respawn position
            volumeBuilder.Color = Colors.Red;
            volumeBuilder.Create(30, -50, 60, 20, 50, 20);

            // Cubes
            var size = 30;

            volumeBuilder.Color = Colors.WhiteSmoke;
            var model = volumeBuilder.Create(0, 0, -50, size);
            volumeBuilder.ApplyTexture(model, VolumeFace.Front, "ki.png");
            model.RotateByAxisY(35);

            volumeBuilder.Color = Colors.WhiteSmoke;
            model = volumeBuilder.Create(50, 0, -50, size);
            volumeBuilder.ApplyTexture(model, VolumeFace.Front, "hon.png");
            model.RotateByAxisY(-35);

            volumeBuilder.Color = Colors.SeaGreen;
            volumeBuilder.Create(0, 0, -10, 10);
            volumeBuilder.Create(36, 0, -50, 10);
            volumeBuilder.Create(70, 0, -10, 10);

            return level;
        }
    }
}
