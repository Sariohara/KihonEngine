using KihonEngine.GameEngine.Graphics.ModelDefinitions;
using Newtonsoft.Json;

namespace KihonEngine.GameEngine.Configuration
{
    public class MapStorageService : IMapStorageService
    {
        public MapDefinition Load(string filepath)
        {
            MapDefinition definition = null;
            if (System.IO.File.Exists(filepath))
            {
                var json = System.IO.File.ReadAllText(filepath);
                definition = JsonConvert.DeserializeObject<MapDefinition>(json);
            }

            return definition;
        }

        public void Save(string filepath, MapDefinition mapDefinition)
        {
            string json = JsonConvert.SerializeObject(mapDefinition, Formatting.Indented);
            System.IO.File.WriteAllText(filepath, json);
        }
    }
}
