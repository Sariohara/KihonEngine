
using KihonEngine.GameEngine.Configuration;
using System.Linq;
using System.Collections.Generic;
using KihonEngine.GameEngine.Graphics.Maps.Predefined;
using KihonEngine.Services;

namespace KihonEngine.GameEngine.Graphics.Maps
{
    public class MapBuilderFactory : IMapBuilderFactory
    {
        private Dictionary<string, IMapBuilder> _mapBuilders;

        public MapBuilderFactory()
        {
            _mapBuilders = new Dictionary<string, IMapBuilder>();
        }

        public void RegisterMap(IMapBuilder mapBuilder)
        {
            _mapBuilders.Add(mapBuilder.MapName, mapBuilder);
        }

        public string[] GetRegisteredMaps()
        {
            return _mapBuilders.Keys.ToArray();
        }

        public IMapBuilder Get(string map)
        {
            if (map.StartsWith("file:"))
            {
                var filepath = map.Substring(5);
                return new MapBuilderFromFile(filepath);
            }

            if (map == PredefinedMapNames.New)
            {
                return new NewMapBuilder();
            }

            if (_mapBuilders.ContainsKey(map))
            {
                return _mapBuilders[map];
            }

            return new EmptyMapBuilder();
        }
    }
}
