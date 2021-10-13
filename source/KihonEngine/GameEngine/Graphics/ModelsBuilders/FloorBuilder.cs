using KihonEngine.GameEngine.Graphics.ModelDefinitions;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
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

        public LayeredModel3D Create(double x, double y, double z, double size)
        {
            return Create(x, y, z, size, size);
        }

        public LayeredModel3D Create(double x, double y, double z, double xSize, double zSize)
        {
            var layeredModel = LayeredModel3D.Create(ModelType.Floor);
            layeredModel.Metadata.Add(ModelType.Floor.ToString(), new FloorMetadata { XSize = xSize, ZSize = zSize, UseBackMaterial = UseBackMaterial });
            layeredModel.Translate(new Vector3D(x, y, z));

            Point3D p0 = new Point3D(0, 0, 0);
            Point3D p1 = new Point3D(xSize, 0, 0);
            Point3D p2 = new Point3D(xSize, 0, zSize);
            Point3D p3 = new Point3D(0, 0, zSize);

            //bottom
            var material = CreateMaterial(null);
            layeredModel.Children.Add(CreateTriangle(p0, p3, p2, material, new[] { new Point(0, 0), new Point(0, 1), new Point(1, 1) }));
            layeredModel.Children.Add(CreateTriangle(p1, p0, p2, material, new[] { new Point(1, 0), new Point(0, 0), new Point(1, 1) }));

            // Metadata
            layeredModel.Metadata.Add("Face1", layeredModel.Children[0]);
            layeredModel.Metadata.Add("Face2", layeredModel.Children[1]);

            models.Add(layeredModel);
            return layeredModel;
        }

        public void ApplyTexture(LayeredModel3D layeredModel, string filename, TileMode tileMode, Stretch stretch, double ratioX, double ratioY)
        {
            var metadata = (FloorMetadata)layeredModel.Metadata[ModelType.Floor.ToString()];
            metadata.Texture = new TextureMetadata { Name = filename, TileMode = tileMode, Stretch = stretch, RatioX = ratioX, RatioY = ratioY };

            var material = CreateMaterial(filename, tileMode, stretch, ratioX, ratioY);
            ApplyTextureToVolume(layeredModel, "Face1", material, TextureCoordinates1);
            ApplyTextureToVolume(layeredModel, "Face2", material, TextureCoordinates2);
        }
    }
}
