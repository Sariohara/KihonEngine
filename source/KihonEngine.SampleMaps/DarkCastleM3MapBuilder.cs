
using KihonEngine.GameEngine.Graphics.Content;
using KihonEngine.GameEngine.Graphics.Maps;
using KihonEngine.Services;

namespace KihonEngine.SampleMaps
{
    public class DarkCastleM3MapBuilder : MapBuilderFromResource
    {
        private IContentService ContentService
            => Container.Get<IContentService>();

        public DarkCastleM3MapBuilder() : base(
            System.Reflection.Assembly.GetExecutingAssembly(),
            "KihonEngine.SampleMaps.DarkCastleM3.json")
        {
            ContentService.RegisterSource(new EmbeddedContentSource(this.GetType()));
        }
    }
}
