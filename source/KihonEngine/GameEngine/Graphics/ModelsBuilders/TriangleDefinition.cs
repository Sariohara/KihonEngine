using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.Graphics.ModelsBuilders
{
    public class TriangleDefinition
    {
        public TriangleDefinition()
        {
            Points = new Point3D[] { };
            TextureCoordinates = new System.Windows.Point[] { };
            Normals = new Vector3D[] { };
        }

        public Point3D[] Points { get; set; }
        public Material Material { get; set; }
        public System.Windows.Point[] TextureCoordinates { get; set; }
        public Vector3D[] Normals { get; set; }
    }
}
