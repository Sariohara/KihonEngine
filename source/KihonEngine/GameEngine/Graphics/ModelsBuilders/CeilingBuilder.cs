using KihonEngine.GameEngine.Graphics.ModelDefinitions;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.Graphics.ModelsBuilders
{
    public class CeilingBuilder : ModelBuilder
    {
        private List<LayeredModel3D> models;
        public CeilingBuilder(Color color, List<LayeredModel3D> models) : base(color)
        {
            this.models = models;
        }

        public LayeredModel3D Create(int x, int y, int z, int xSize, int zSize)
        {
            var layeredModel = LayeredModel3D.Create(ModelType.Ceiling);
            layeredModel.Metadata.Add(ModelType.Ceiling.ToString(), new CeilingMetadata { XSize = xSize, ZSize = zSize, UseBackMaterial = UseBackMaterial });
            layeredModel.Translate(new Vector3D(x, y, z));

            Point3D p0 = new Point3D(0, 0, 0);
            Point3D p1 = new Point3D(xSize, 0, 0);
            Point3D p2 = new Point3D(xSize, 0, zSize);
            Point3D p3 = new Point3D(0, 0, zSize);

            //bottom
            layeredModel.Children.Add(CreateTriangle(p0, p1, p2));
            layeredModel.Children.Add(CreateTriangle(p2, p3, p0));

            models.Add(layeredModel);
            return layeredModel;
        }
    }
}
