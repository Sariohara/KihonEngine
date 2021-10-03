
using KihonEngine.GameEngine.Graphics.Maps;

namespace KihonEngine.SampleMaps
{
    public class AbstractCityMapBuilder : MapBuilderFromResource
    {
        public AbstractCityMapBuilder() : base(
            System.Reflection.Assembly.GetExecutingAssembly(),
            "KihonEngine.SampleMaps.AbstractCity.json")
        {
        }
    }
}
