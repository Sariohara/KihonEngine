using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.Graphics.Maps
{
    public class MapBuilderFromModels : IMapBuilder
    {
        public string MapName { get; set; }

        public Point3D PlayerPosition { get; set; }

        public Vector3D PlayerLookDirection { get; set; }

        private List<LayeredModel3D> _models;

        public MapBuilderFromModels(List<LayeredModel3D> models)
        {
            _models = models;
        }

        public List<LayeredModel3D> CreateMap()
        {
            return _models;
        }
    }
}
