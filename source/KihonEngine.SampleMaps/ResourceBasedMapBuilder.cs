using KihonEngine.GameEngine.Graphics.Maps;
using KihonEngine.GameEngine.Graphics.ModelDefinitions;
using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Media3D;

namespace KihonEngine.SampleMaps
{
    public abstract class ResourceBasedMapBuilder : IMapBuilder
    {
        public string MapName => _mapBuilder.MapName;

        public Point3D PlayerPosition => _mapBuilder.PlayerPosition;

        public Vector3D PlayerLookDirection => _mapBuilder.PlayerLookDirection;

        private MapBuilderFromDefinition _mapBuilder;

        public ResourceBasedMapBuilder(string resourceName)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var assemblyName = typeof(ResourceBasedMapBuilder).Assembly.GetName().Name;
            var stream = assembly.GetManifestResourceStream($"{assemblyName}.{resourceName}");
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
