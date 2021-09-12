using KihonEngine.GameEngine.Graphics.ModelDefinitions;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.Graphics.ModelsBuilders
{
    public class SkyboxBuilder : ModelBuilder
    {
        private List<LayeredModel3D> models;

        public SkyboxBuilder(Color color, List<LayeredModel3D> models) : base(color)
        {
            this.models = models;
        }

        public LayeredModel3D Create(int x, int y, int z, int w, Vector3D normal, string skyboxName = null)
        {
            var layeredModel = LayeredModel3D.Create(ModelType.Skybox);
            layeredModel.Metadata.Add(ModelType.Skybox.ToString(), new SkyboxMetadata { Name = skyboxName, Width = w, Normal = normal, UseBackMaterial = UseBackMaterial });
            layeredModel.Translate(new Vector3D(x, y, z));
            //layeredModel.Translation = TransformHelper.TransformByTranslation(x, y, z);

            Point3D p0 = new Point3D(0, 0, 0);
            Point3D p1 = new Point3D(w, 0, 0);
            Point3D p2 = new Point3D(w, 0, w);
            Point3D p3 = new Point3D(0, 0, w);
            Point3D p4 = new Point3D(0, w, 0);
            Point3D p5 = new Point3D(w, w, 0);
            Point3D p6 = new Point3D(w, w, w);
            Point3D p7 = new Point3D(0, w, w);

            //front
            layeredModel.Children.Add(CreateTriangle(p6, p2, p3, CreateMaterial(skyboxName, "back"), new[] { new Point(0, 0), new Point(0, 1), new Point(1, 1) }, new[] { normal, normal, normal }));
            layeredModel.Children.Add(CreateTriangle(p7, p6, p3, CreateMaterial(skyboxName, "back"), new[] { new Point(1, 0), new Point(0, 0), new Point(1, 1) }, new[] { normal, normal, normal }));

            //right                                                               
            layeredModel.Children.Add(CreateTriangle(p5, p1, p2, CreateMaterial(skyboxName, "right"), new[] { new Point(0, 0), new Point(0, 1), new Point(1, 1) }, new[] { normal, normal, normal}));
            layeredModel.Children.Add(CreateTriangle(p6, p5, p2, CreateMaterial(skyboxName, "right"), new[] { new Point(1, 0), new Point(0, 0), new Point(1, 1) }, new[] { normal, normal, normal }));

            //back                                                                
            layeredModel.Children.Add(CreateTriangle(p4, p0, p1, CreateMaterial(skyboxName, "front"), new[] { new Point(0, 0), new Point(0, 1), new Point(1, 1) }, new[] { normal, normal, normal }));
            layeredModel.Children.Add(CreateTriangle(p5, p4, p1, CreateMaterial(skyboxName, "front"), new[] { new Point(1, 0), new Point(0, 0), new Point(1, 1) }, new[] { normal, normal, normal }));

            //left                                                                
            layeredModel.Children.Add(CreateTriangle(p7, p3, p0, CreateMaterial(skyboxName, "left"), new[] { new Point(0, 0), new Point(0, 1), new Point(1, 1) }, new[] { normal, normal, normal }));
            layeredModel.Children.Add(CreateTriangle(p4, p7, p0, CreateMaterial(skyboxName, "left"), new[] { new Point(1, 0), new Point(0, 0), new Point(1, 1) }, new[] { normal, normal, normal }));

            //top                                                                 
            layeredModel.Children.Add(CreateTriangle(p5, p6, p7, CreateMaterial(skyboxName, "top"), new[] { new Point(1, 1), new Point(1, 0), new Point(0, 0) }, new[] { normal, normal, normal }));
            layeredModel.Children.Add(CreateTriangle(p4, p5, p7, CreateMaterial(skyboxName, "top"), new[] { new Point(0, 1), new Point(1, 1), new Point(0, 0) }, new[] { normal, normal, normal }));

            //bottom                                                              
            layeredModel.Children.Add(CreateTriangle(p0, p3, p2, CreateMaterial(skyboxName, "bottom"), new[] { new Point(0, 0), new Point(0, 1), new Point(1, 1) }, new[] { normal, normal, normal }));
            layeredModel.Children.Add(CreateTriangle(p1, p0, p2, CreateMaterial(skyboxName, "bottom"), new[] { new Point(1, 0), new Point(0, 0), new Point(1, 1) }, new[] { normal, normal, normal }));

            models.Add(layeredModel);
            return layeredModel;
        }

        private Material CreateMaterial(string filepath, string face)
        {
            var materiaGroup = new MaterialGroup();

            if (!string.IsNullOrEmpty(filepath))
            {
                var imageSource = ImageHelper.Get($"Skyboxes.{filepath}-{face}.png");
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
