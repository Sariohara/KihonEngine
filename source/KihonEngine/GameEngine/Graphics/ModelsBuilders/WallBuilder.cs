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
            layeredModel.Metadata.Add(ModelType.Wall.ToString(), new WallMetadata { Face = VolumeFace.Front, XSize = xSize, YSize = ySize, UseBackMaterial = UseBackMaterial });

            SetFace(layeredModel, VolumeFace.Front, material);
            //Point3D p0 = new Point3D(0, 0, 0);
            //Point3D p1 = new Point3D(xSize, 0, 0);
            //Point3D p4 = new Point3D(0, ySize, 0);
            //Point3D p5 = new Point3D(xSize, ySize, 0);

            ////back
            //layeredModel.Children.Add(CreateTriangle(p1, p0, p4, material));
            //layeredModel.Children.Add(CreateTriangle(p1, p4, p5, material));

            //// Metadata
            //layeredModel.Metadata.Add("Face1", layeredModel.Children[0]);
            //layeredModel.Metadata.Add("Face2", layeredModel.Children[1]);

            models.Add(layeredModel);
            return layeredModel;
        }

        public void SetFace(LayeredModel3D layeredModel, VolumeFace face, Material material = null)
        {
            var metadata = (WallMetadata)layeredModel.Metadata[ModelType.Wall.ToString()];
            metadata.Face = face;

            if (material == null && layeredModel.Children.Count > 0)
            {
                var model3DGroup = (Model3DGroup)layeredModel.Children[0];
                var geometryModel = (GeometryModel3D)model3DGroup.Children[0];
                material = geometryModel.Material;
            }

            layeredModel.Children.Clear();

            var xSize = metadata.XSize;
            var ySize = metadata.YSize;
            var zSize = metadata.XSize;

            if (face == VolumeFace.Front)
            {
                zSize = 0;
                Point3D p2 = new Point3D(xSize, 0, zSize);
                Point3D p3 = new Point3D(0, 0, zSize);
                Point3D p6 = new Point3D(xSize, ySize, zSize);
                Point3D p7 = new Point3D(0, ySize, zSize);

                //front
                layeredModel.Children.Add(CreateTriangle(p3, p2, p6, material));
                layeredModel.Children.Add(CreateTriangle(p3, p6, p7, material));
            }
            else if (face == VolumeFace.Back)
            {
                Point3D p0 = new Point3D(0, 0, 0);
                Point3D p1 = new Point3D(xSize, 0, 0);
                Point3D p4 = new Point3D(0, ySize, 0);
                Point3D p5 = new Point3D(xSize, ySize, 0);

                //back
                layeredModel.Children.Add(CreateTriangle(p1, p0, p4, material));
                layeredModel.Children.Add(CreateTriangle(p1, p4, p5, material));
            }
            else if (face == VolumeFace.Left)
            {
                xSize = 0;
                Point3D p1 = new Point3D(xSize, 0, 0);
                Point3D p2 = new Point3D(xSize, 0, zSize);
                Point3D p5 = new Point3D(xSize, ySize, 0);
                Point3D p6 = new Point3D(xSize, ySize, zSize);

                //right
                layeredModel.Children.Add(CreateTriangle(p2, p1, p5, material));
                layeredModel.Children.Add(CreateTriangle(p2, p5, p6, material));
            }
            else if (face == VolumeFace.Right)
            {
                Point3D p0 = new Point3D(0, 0, 0);
                Point3D p3 = new Point3D(0, 0, zSize);
                Point3D p4 = new Point3D(0, ySize, 0);
                Point3D p7 = new Point3D(0, ySize, zSize);

                //left
                layeredModel.Children.Add(CreateTriangle(p0, p3, p7, material));
                layeredModel.Children.Add(CreateTriangle(p0, p7, p4, material));
            }

            // Metadata
            layeredModel.Metadata["Face1"] = layeredModel.Children[0];
            layeredModel.Metadata["Face2"] = layeredModel.Children[1];
        }

        public void ApplyTexture(LayeredModel3D layeredModel, string filename, TileMode tileMode, Stretch stretch, double ratioX, double ratioY)
        {
            var metadata = (WallMetadata)layeredModel.Metadata[ModelType.Wall.ToString()];
            metadata.Texture = new TextureMetadata { Name = filename, TileMode = tileMode, Stretch = stretch, RatioX = ratioX, RatioY = ratioY };

            var material = CreateMaterial(filename, tileMode, stretch, ratioX, ratioY);
            ApplyTextureToVolume(layeredModel, "Face1", material, TextureCoordinates1);
            ApplyTextureToVolume(layeredModel, "Face2", material, TextureCoordinates2);
        }
    }
}
