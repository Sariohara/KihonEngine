using KihonEngine.GameEngine.Graphics.ModelDefinitions;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.Graphics.ModelsBuilders
{
    public class VolumeBuilder : ModelBuilder
    {
        private List<LayeredModel3D> models;
        public VolumeBuilder(Color color, List<LayeredModel3D> models) : base(color)
        {
            this.models = models;
        }

        public LayeredModel3D Create(double x, double y, double z, Material material = null)
        {
            return Create(x, y, z, 5, 5, 5, material);
        }

        public LayeredModel3D Create(double x, double y, double z, double w, Material material = null)
        {
            return Create(x, y, z, w, w, w, material);
        }

        public LayeredModel3D Create(double x, double y, double z, double w, double l, double h, Material material = null)
        {
            w = Math.Round(w, 2);
            h = Math.Round(h, 2);
            l = Math.Round(l, 2);

            var layeredModel = LayeredModel3D.Create(ModelType.Volume);
            layeredModel.Metadata.Add(ModelType.Volume.ToString(), new VolumeMetadata { Width = w, Height = h, Length = l, UseBackMaterial = UseBackMaterial });
            layeredModel.Translate(new Vector3D(x, y, z));
            //layeredModel.Translation = TransformHelper.TransformByTranslation(x, y, z);

            Point3D p0 = new Point3D(0, 0, 0);
            Point3D p1 = new Point3D(w, 0, 0);
            Point3D p2 = new Point3D(w, 0, l);
            Point3D p3 = new Point3D(0, 0, l);
            Point3D p4 = new Point3D(0, h, 0);
            Point3D p5 = new Point3D(w, h, 0);
            Point3D p6 = new Point3D(w, h, l);
            Point3D p7 = new Point3D(0, h, l);

            //front
            layeredModel.Children.Add(CreateTriangle(p3, p2, p6, material));
            layeredModel.Children.Add(CreateTriangle(p3, p6, p7, material));

            //right
            layeredModel.Children.Add(CreateTriangle(p2, p1, p5, material));
            layeredModel.Children.Add(CreateTriangle(p2, p5, p6, material));

            //back
            layeredModel.Children.Add(CreateTriangle(p1, p0, p4, material));
            layeredModel.Children.Add(CreateTriangle(p1, p4, p5, material));

            //left
            layeredModel.Children.Add(CreateTriangle(p0, p3, p7, material));
            layeredModel.Children.Add(CreateTriangle(p0, p7, p4, material));

            //top
            layeredModel.Children.Add(CreateTriangle(p7, p6, p5, material));
            layeredModel.Children.Add(CreateTriangle(p7, p5, p4, material));

            //bottom
            layeredModel.Children.Add(CreateTriangle(p2, p3, p0, material));
            layeredModel.Children.Add(CreateTriangle(p2, p0, p1, material));

            // Metadata
            layeredModel.Metadata.Add("Front1", layeredModel.Children[0]);
            layeredModel.Metadata.Add("Front2", layeredModel.Children[1]);
            layeredModel.Metadata.Add("Right1", layeredModel.Children[2]);
            layeredModel.Metadata.Add("Right2", layeredModel.Children[3]);
            layeredModel.Metadata.Add("Back1", layeredModel.Children[4]);
            layeredModel.Metadata.Add("Back2", layeredModel.Children[5]);
            layeredModel.Metadata.Add("Left1", layeredModel.Children[6]);
            layeredModel.Metadata.Add("Left2", layeredModel.Children[7]);
            layeredModel.Metadata.Add("Top1", layeredModel.Children[8]);
            layeredModel.Metadata.Add("Top2", layeredModel.Children[9]);
            layeredModel.Metadata.Add("Bottom1", layeredModel.Children[10]);
            layeredModel.Metadata.Add("Bottom2", layeredModel.Children[11]);

            models.Add(layeredModel);
            return layeredModel;
        }
    }
}
