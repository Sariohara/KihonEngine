
using KihonEngine.GameEngine.Graphics.Maps;

namespace KihonEngine.SampleMaps
{
    public class E1M1MapBuilder : MapBuilderFromResource
    {
        public E1M1MapBuilder() : base(
            System.Reflection.Assembly.GetExecutingAssembly(),
            "KihonEngine.SampleMaps.E1M1.json")
        {
        }
    }
}
