using KihonEngine.GameEngine.Graphics.ModelDefinitions;
using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.Graphics.Maps
{
    public abstract class MapBuilderFromResource : IMapBuilder
    {
        public string MapName => _mapBuilder.MapName;

        public Point3D PlayerPosition => _mapBuilder.PlayerPosition;

        public Vector3D PlayerLookDirection => _mapBuilder.PlayerLookDirection;

        private MapBuilderFromDefinition _mapBuilder;

        public MapBuilderFromResource(System.Reflection.Assembly resourceAssembly, string resourceName)
        {
            var assemblyName = resourceAssembly.GetName().Name;
            var stream = resourceAssembly.GetManifestResourceStream(resourceName);
            using (StreamReader reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                var mapDefinition = JsonConvert.DeserializeObject<MapDefinition>(json);
                _mapBuilder = new MapBuilderFromDefinition(mapDefinition);
            }
        }

        public List<LayeredModel3D> CreateMap()
        {
            return _mapBuilder.CreateMap();
        }
    }
}
