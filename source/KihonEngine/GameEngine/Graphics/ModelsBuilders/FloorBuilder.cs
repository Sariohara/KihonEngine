using KihonEngine.GameEngine.Graphics.ModelDefinitions;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.Graphics.ModelsBuilders
{
    public class FloorBuilder : ModelBuilder
    {
        private List<LayeredModel3D> models;
        public FloorBuilder(Color color, List<LayeredModel3D> models) : base(color)
        {
            this.models = models;
        }

        public LayeredModel3D Create(double x, double y, double z, double w, string texture = null)
        {
            return Create(x, y, z, w, w, texture);
        }

        public LayeredModel3D Create(double x, double y, double z, double w, double l, string texture = null)
        {
            var layeredModel = LayeredModel3D.Create(ModelType.Floor);
            layeredModel.Metadata.Add(ModelType.Floor.ToString(), new FloorMetadata { Texture = texture, Width = w, Length = l, UseBackMaterial = UseBackMaterial });
            layeredModel.Translate(new Vector3D(x, y, z));

            Point3D p0 = new Point3D(0, 0, 0);
            Point3D p1 = new Point3D(w, 0, 0);
            Point3D p2 = new Point3D(w, 0, l);
            Point3D p3 = new Point3D(0, 0, l);

            //bottom
            layeredModel.Children.Add(CreateTriangle(p0, p3, p2, CreateMaterial(texture), new[] { new Point(0, 0), new Point(0, 1), new Point(1, 1) }));
            layeredModel.Children.Add(CreateTriangle(p1, p0, p2, CreateMaterial(texture), new[] { new Point(1, 0), new Point(0, 0), new Point(1, 1) }));

            models.Add(layeredModel);
            return layeredModel;
        }

        private Material CreateMaterial(string filename)
        {
            var materiaGroup = new MaterialGroup();

            if (!string.IsNullOrEmpty(filename))
            {
                var imageSource = ImageHelper.Get($"Textures.{filename}");
                var brush = new ImageBrush(imageSource);
                brush.TileMode = TileMode.Tile;
                brush.Stretch = Stretch.Uniform;
                brush.Viewport = new Rect(new Point(0, 0), new Point(0.05, 0.05));
                brush.ViewboxUnits = BrushMappingMode.RelativeToBoundingBox;
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
