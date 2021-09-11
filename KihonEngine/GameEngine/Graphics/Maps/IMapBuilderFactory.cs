
namespace KihonEngine.GameEngine.Graphics.Maps
{
    public interface IMapBuilderFactory
    {
        void RegisterMap(IMapBuilder mapBuilder);
        string[] GetRegisteredMaps();
        IMapBuilder Get(string map);
    }
}
