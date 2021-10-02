
using KihonEngine.GameEngine.Graphics.Maps;

namespace KihonEngine.SampleMaps
{
    public class DarkCastleM2MapBuilder : MapBuilderFromResource
    {
        public DarkCastleM2MapBuilder() : base(
            System.Reflection.Assembly.GetExecutingAssembly(),
            "KihonEngine.SampleMaps.DarkCastleM2.json")
        {
        }
    }
}
