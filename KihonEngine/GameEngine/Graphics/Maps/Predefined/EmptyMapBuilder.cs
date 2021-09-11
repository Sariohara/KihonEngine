using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.Graphics.Maps.Predefined
{
    public class EmptyMapBuilder : IMapBuilder
    {
        public string MapName => PredefinedMapNames.Empty;

        public Point3D PlayerPosition => new Point3D(10, 10, 70);

        public Vector3D PlayerLookDirection => new Vector3D(0, 0, -1);

        public List<LayeredModel3D> CreateMap()
        {
            var level = new List<LayeredModel3D>();

            return level;
        }
    }
}
