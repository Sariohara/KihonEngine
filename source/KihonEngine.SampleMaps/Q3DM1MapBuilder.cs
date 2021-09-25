
using KihonEngine.GameEngine.Graphics.Maps;

namespace KihonEngine.SampleMaps
{
    public class Q3DM1MapBuilder : MapBuilderFromResource
    {
        public Q3DM1MapBuilder() : base(
            System.Reflection.Assembly.GetExecutingAssembly(), 
            "KihonEngine.SampleMaps.Q3DM1.json")
        {
        }
    }
}
