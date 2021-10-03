using KihonEngine.GameEngine.Graphics.ModelDefinitions;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.Graphics.ModelsBuilders
{
    public class WallBuilder : ModelBuilder
    {
        private List<LayeredModel3D> models;
        public WallBuilder(Color color, List<LayeredModel3D> models) : base(color)
        {
            this.models = models;
        }

        public LayeredModel3D Create(int x, int y, int z, int xSize, int ySize, /*double d,*/ Material material = null)
        {
            var layeredModel = LayeredModel3D.Create(ModelType.Wall);
            layeredModel.Translate(new Vector3D(x, y, z));
            layeredModel.Metadata.Add(ModelType.Wall.ToString(), new WallMetadata { XSize = xSize, YSize = ySize, UseBackMaterial = UseBackMaterial });

            Point3D p0 = new Point3D(0, 0, 0);
            Point3D p2 = new Point3D(xSize, 0, 0);
            Point3D p1 = new Point3D(0, ySize, 0);
            Point3D p3 = new Point3D(xSize, ySize, 0);

            //back
            layeredModel.Children.Add(CreateTriangle(p1, p0, p2, material));
            layeredModel.Children.Add(CreateTriangle(p3, p1, p2, material));

            //// Transformations
            //layeredModel.Translation = TransformHelper.TransformByTranslation(x, y, z);

            models.Add(layeredModel);
            return layeredModel;
        }
    }
}
