using KihonEngine.GameEngine.Graphics.ModelDefinitions;

namespace KihonEngine.GameEngine.Configuration
{
    public interface IMapStorageService
    {
        MapDefinition Load(string filepath);
        void Save(string filepath, MapDefinition mapDefinition);
    }
}
