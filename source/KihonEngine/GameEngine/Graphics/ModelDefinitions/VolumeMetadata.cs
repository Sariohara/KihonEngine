
namespace KihonEngine.GameEngine.Graphics.ModelDefinitions
{
    public class VolumeMetadata
    {
        public double XSize { get; set; }
        public double YSize { get; set; }
        public double ZSize { get; set; }
        public TextureMetadata FaceTexture { get; set; }
        public TextureMetadata BackTexture { get; set; }
        public TextureMetadata LeftTexture { get; set; }
        public TextureMetadata RightTexture { get; set; }
        public TextureMetadata TopTexture { get; set; }
        public TextureMetadata BottomTexture { get; set; }
        public bool UseBackMaterial { get; set; }
        public double? Opacity { get; set; }
    }
}
