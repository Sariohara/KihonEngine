using System.Collections.Generic;
using System.Windows.Media.Media3D;
using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using KihonEngine.GameEngine.Configuration;
using KihonEngine.Services;

namespace KihonEngine.GameEngine.Graphics.Maps
{
    public class MapBuilderFromFile : IMapBuilder
    {
        public string MapName => _mapBuilder.MapName;

        public Point3D PlayerPosition => _mapBuilder.PlayerPosition;

        public Vector3D PlayerLookDirection => _mapBuilder.PlayerLookDirection;

        private IMapStorageService MapStorageService
            => Container.Get<IMapStorageService>();

        private MapBuilderFromDefinition _mapBuilder;

        public MapBuilderFromFile(string filepath)
        {
            var mapDefinition = MapStorageService.Load(filepath);

            _mapBuilder = new MapBuilderFromDefinition(mapDefinition);
        }

        public List<LayeredModel3D> CreateMap()
        {
            return _mapBuilder.CreateMap();
        }
    }
}
