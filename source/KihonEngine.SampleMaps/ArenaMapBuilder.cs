
using KihonEngine.GameEngine.Graphics.Maps;

namespace KihonEngine.SampleMaps
{
    public class ArenaMapBuilder : MapBuilderFromResource
    {
        public ArenaMapBuilder() : base(
            System.Reflection.Assembly.GetExecutingAssembly(),
            "KihonEngine.SampleMaps.Arena.json")
        {
        }
    }
}
