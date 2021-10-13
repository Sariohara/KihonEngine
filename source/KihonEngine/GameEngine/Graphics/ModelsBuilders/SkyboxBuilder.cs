using KihonEngine.GameEngine.Graphics.ModelDefinitions;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.Graphics.ModelsBuilders
{
    public class SkyboxBuilder : ModelBuilder
    {
        private List<LayeredModel3D> models;

        private Point[] _defaultTextureCoordinates1 = new[] { new Point(0, 0), new Point(0, 1), new Point(1, 1) };
        private Point[] _defaultTextureCoordinates2 = new[] { new Point(1, 0), new Point(0, 0), new Point(1, 1) };
        private Point[] _topTextureCoordinates1 = new[] { new Point(1, 1), new Point(1, 0), new Point(0, 0) };
        private Point[] _topTextureCoordinates2 = new[] { new Point(0, 1), new Point(1, 1), new Point(0, 0) };

        public SkyboxBuilder(Color color, List<LayeredModel3D> models) : base(color)
        {
            this.models = models;
        }

        public LayeredModel3D Create(int x, int y, int z, int size, Vector3D normal, string skyboxName = null)
        {
            var layeredModel = LayeredModel3D.Create(ModelType.Skybox);
            layeredModel.Metadata.Add(ModelType.Skybox.ToString(), new SkyboxMetadata { Name = skyboxName, Size = size, Normal = normal, UseBackMaterial = UseBackMaterial });
            layeredModel.Translate(new Vector3D(x, y, z));
            //layeredModel.Translation = TransformHelper.TransformByTranslation(x, y, z);

            Point3D p0 = new Point3D(0, 0, 0);
            Point3D p1 = new Point3D(size, 0, 0);
            Point3D p2 = new Point3D(size, 0, size);
            Point3D p3 = new Point3D(0, 0, size);
            Point3D p4 = new Point3D(0, size, 0);
            Point3D p5 = new Point3D(size, size, 0);
            Point3D p6 = new Point3D(size, size, size);
            Point3D p7 = new Point3D(0, size, size);

            var normals = new[] { normal, normal, normal };

            //front
            layeredModel.Children.Add(CreateTriangle(p6, p2, p3, CreateMaterial(skyboxName, SkyboxFace.Back), _defaultTextureCoordinates1, normals));
            layeredModel.Children.Add(CreateTriangle(p7, p6, p3, CreateMaterial(skyboxName, SkyboxFace.Back), _defaultTextureCoordinates2, normals));

            //right                                                               
            layeredModel.Children.Add(CreateTriangle(p5, p1, p2, CreateMaterial(skyboxName, SkyboxFace.Right), _defaultTextureCoordinates1, normals));
            layeredModel.Children.Add(CreateTriangle(p6, p5, p2, CreateMaterial(skyboxName, SkyboxFace.Right), _defaultTextureCoordinates2, normals));

            //back                                                                
            layeredModel.Children.Add(CreateTriangle(p4, p0, p1, CreateMaterial(skyboxName, SkyboxFace.Front), _defaultTextureCoordinates1, normals));
            layeredModel.Children.Add(CreateTriangle(p5, p4, p1, CreateMaterial(skyboxName, SkyboxFace.Front), _defaultTextureCoordinates2, normals));

            //left                                                                
            layeredModel.Children.Add(CreateTriangle(p7, p3, p0, CreateMaterial(skyboxName, SkyboxFace.Left), _defaultTextureCoordinates1, normals));
            layeredModel.Children.Add(CreateTriangle(p4, p7, p0, CreateMaterial(skyboxName, SkyboxFace.Left), _defaultTextureCoordinates2, normals));

            //top                                                                 
            layeredModel.Children.Add(CreateTriangle(p5, p6, p7, CreateMaterial(skyboxName, SkyboxFace.Top), _topTextureCoordinates1, normals));
            layeredModel.Children.Add(CreateTriangle(p4, p5, p7, CreateMaterial(skyboxName, SkyboxFace.Top), _topTextureCoordinates2, normals));

            //bottom                                                              
            layeredModel.Children.Add(CreateTriangle(p0, p3, p2, CreateMaterial(skyboxName, SkyboxFace.Bottom), _defaultTextureCoordinates1, normals));
            layeredModel.Children.Add(CreateTriangle(p1, p0, p2, CreateMaterial(skyboxName, SkyboxFace.Bottom), _defaultTextureCoordinates2, normals));

            models.Add(layeredModel);
            return layeredModel;
        }

        private Material CreateMaterial(string filepath, SkyboxFace face)
        {
            var materiaGroup = new MaterialGroup();

            if (!string.IsNullOrEmpty(filepath))
            {
                var imageSource = ImageHelper.GetSkyboxPart(filepath, face);
                var brush = new ImageBrush(imageSource);
                materiaGroup.Children.Add(new DiffuseMaterial(brush));
            }
            else
            {
                materiaGroup.Children.Add(new DiffuseMaterial(new SolidColorBrush(Color)));
            }

            return materiaGroup;
        }
    }
}
