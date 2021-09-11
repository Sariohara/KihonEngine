using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.Graphics.ModelDefinitions
{
    public class SkyboxMetadata
    {
        public string Name { get; set; }
        public int Width { get; set; }
        public Vector3D Normal { get; set; }
        public bool UseBackMaterial { get; set; }
    }
}
