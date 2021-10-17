
namespace KihonEngine.GameEngine.Graphics.ModelDefinitions
{
    public class VolumeMetadata
    {
        public double XSize { get; set; }
        public double YSize { get; set; }
        public double ZSize { get; set; }
        public TextureMetadata TextureFront { get; set; }
        public TextureMetadata TextureBack { get; set; }
        public TextureMetadata TextureLeft { get; set; }
        public TextureMetadata TextureRight { get; set; }
        public TextureMetadata TextureTop { get; set; }
        public TextureMetadata TextureBottom { get; set; }
        public bool UseBackMaterial { get; set; }
        public double? Opacity { get; set; }
    }
}
