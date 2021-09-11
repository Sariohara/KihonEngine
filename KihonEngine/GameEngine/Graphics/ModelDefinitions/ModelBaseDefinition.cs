using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.Graphics.ModelDefinitions
{
    public abstract class ModelBaseDefinition
    {
        public int Index { get; set; }
        public Color Color { get; set; }
        public Point3D Position { get; set; }
        public double RotationAxisX { get; set; }
        public double RotationAxisY { get; set; }
        public double RotationAxisZ { get; set; }
    }
}
