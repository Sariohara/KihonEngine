using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.Graphics.ModelsBuilders
{
    public class ModelBuilder
    {
        protected Point[] TextureCoordinates1 = new[] { new Point(0, 1), new Point(1, 1), new Point(1, 0) };
        protected Point[] TextureCoordinates2 = new[] { new Point(0, 1), new Point(1, 0), new Point(0, 0) };

        public Color Color { get; set; }
        public bool UseBackMaterial { get; set; }

        public ModelBuilder(Color color)
        {
            Color = color;
        }

        public Model3DGroup CreateTriangle(TriangleDefinition definition)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions.Add(definition.Points[0]);
            mesh.Positions.Add(definition.Points[1]);
            mesh.Positions.Add(definition.Points[2]);
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(2);

            // Add material
            var material = definition.Material;
            if (material == null)
            {
                var materialGroup = new MaterialGroup();
                materialGroup.Children.Add(new DiffuseMaterial(new SolidColorBrush(Color)));
                material = materialGroup;
            }

            // Set texture
            if (definition.TextureCoordinates.Count() > 0)
            {
                foreach (var point in definition.TextureCoordinates)
                {
                    mesh.TextureCoordinates.Add(point);
                }
            }

            // Set normals
            if (definition.Normals?.Count() > 0)
            {
                foreach(var normal in definition.Normals)
                {
                    mesh.Normals.Add(normal);
                }
            }

            GeometryModel3D model = new GeometryModel3D(mesh, material);

            if (UseBackMaterial)
            {
                model.BackMaterial = material;
            }

            Model3DGroup group = new Model3DGroup();
            group.Children.Add(model);
            return group;
        }

        public Model3DGroup CreateTriangle(Point3D p0, Point3D p1, Point3D p2, Material material, System.Windows.Point[] textureCoordinates, Vector3D[] normals = null)
        {
            return CreateTriangle(new TriangleDefinition
            {
                Points = new[] { p0, p1, p2 },
                Material = material,
                TextureCoordinates = textureCoordinates,
                Normals = normals,
            });
        }

        public Model3DGroup CreateTriangle(Point3D p0, Point3D p1, Point3D p2, Material material)
        {
            return CreateTriangle(new TriangleDefinition
            {
                Points = new[] { p0, p1, p2 },
                Material = material,
            });
        }

        public Model3DGroup CreateTriangle(Point3D p0, Point3D p1, Point3D p2)
        {
            return CreateTriangle(new TriangleDefinition 
            { 
                Points = new[] { p0, p1, p2} 
            });
        }

        protected MaterialGroup CreateMaterial(string filename, TileMode tileMode = TileMode.Tile, Stretch stretch = Stretch.Uniform, double ratio = 1)
        {
            MaterialGroup material;
            if (!string.IsNullOrEmpty(filename))
            {
                material = ImageHelper.CreateMaterial(filename, tileMode, stretch, ratio);
            }
            else
            {
                material = new MaterialGroup();
                material.Children.Add(new DiffuseMaterial(new SolidColorBrush(Color)));
            }

            return material;
        }

        protected void ApplyTextureToVolume(LayeredModel3D layeredModel, string face, MaterialGroup material, Point[] textureCoordinates)
        {
            var model3DGroup = (Model3DGroup)layeredModel.Metadata[face];
            var geometry = (GeometryModel3D)model3DGroup.Children[0];
            geometry.Material = material;
            var mesh = (MeshGeometry3D)geometry.Geometry;

            foreach (var point in textureCoordinates)
            {
                mesh.TextureCoordinates.Add(point);
            }
        }
    }
}
