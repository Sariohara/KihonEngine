using System.Windows.Media;

namespace KihonEngine.GameEngine.Graphics.ModelDefinitions
{
    public class TextureMetadata
    {
        public string Name { get; set; }
        public TileMode TileMode { get; set; }
        public Stretch Stretch { get; set; }
        public double RatioX { get; set; }
        public double RatioY { get; set; }
    }
}
