using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.State.Editor
{
    public class SelectModelState
    {
        public LayeredModel3D SelectedModel { get; set; }

        public Point3D InitialModelPosition { get; set; }
        public Point3D? HitPoint { get; set; }
        public double MinX { get; set; }
        public double MinY { get; set; }
        public double MinZ { get; set; }
        public double MaxX { get; set; }
        public double MaxY { get; set; }
        public double MaxZ { get; set; }
        public LayeredModel3D SelectionBoxModel { get; set; }
    }
}
