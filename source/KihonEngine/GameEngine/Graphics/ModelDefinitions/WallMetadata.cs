
using KihonEngine.GameEngine.Graphics.ModelsBuilders;

namespace KihonEngine.GameEngine.Graphics.ModelDefinitions
{
    public class WallMetadata
    {
        public VolumeFace Face { get; set; }
        public double XSize { get; set; }
        public double YSize { get; set; }
        public TextureMetadata Texture { get; set; }
        public bool UseBackMaterial { get; set; }
    }
}
