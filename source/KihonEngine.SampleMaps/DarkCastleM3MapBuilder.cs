
using KihonEngine.GameEngine.Graphics.Maps;

namespace KihonEngine.SampleMaps
{
    public class DarkCastleM3MapBuilder : MapBuilderFromResource
    {
        public DarkCastleM3MapBuilder() : base(
            System.Reflection.Assembly.GetExecutingAssembly(),
            "KihonEngine.SampleMaps.DarkCastleM3.json")
        {
        }
    }
}
