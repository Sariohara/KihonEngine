﻿using KihonEngine.GameEngine.Graphics.ModelDefinitions;
using System;
using System.Collections.Generic;
using System.Windows;
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

        public LayeredModel3D Create(double x, double y, double z, double size, Material material = null)
        {
            return Create(x, y, z, size, size, size, material);
        }

        public LayeredModel3D Create(double x, double y, double z, double xSize, double ySize, double zSize, Material material = null)
        {
            xSize = Math.Round(xSize, 2);
            ySize = Math.Round(ySize, 2);
            zSize = Math.Round(zSize, 2);

            var layeredModel = LayeredModel3D.Create(ModelType.Volume);
            SetGenericMetadata(layeredModel);
            SetSpecificMetadata(layeredModel, new VolumeMetadata { XSize = xSize, YSize = ySize, ZSize = zSize, UseBackMaterial = UseBackMaterial });
            layeredModel.Translate(new Vector3D(x, y, z));
            //layeredModel.Translation = TransformHelper.TransformByTranslation(x, y, z);

            Point3D p0 = new Point3D(0, 0, 0);
            Point3D p1 = new Point3D(xSize, 0, 0);
            Point3D p2 = new Point3D(xSize, 0, zSize);
            Point3D p3 = new Point3D(0, 0, zSize);
            Point3D p4 = new Point3D(0, ySize, 0);
            Point3D p5 = new Point3D(xSize, ySize, 0);
            Point3D p6 = new Point3D(xSize, ySize, zSize);
            Point3D p7 = new Point3D(0, ySize, zSize);

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
            layeredModel.Metadata.Add($"{VolumeFace.Front}1", layeredModel.Children[0]);
            layeredModel.Metadata.Add($"{VolumeFace.Front}2", layeredModel.Children[1]);
            layeredModel.Metadata.Add($"{VolumeFace.Right}1", layeredModel.Children[2]);
            layeredModel.Metadata.Add($"{VolumeFace.Right}2", layeredModel.Children[3]);
            layeredModel.Metadata.Add($"{VolumeFace.Back}1", layeredModel.Children[4]);
            layeredModel.Metadata.Add($"{VolumeFace.Back}2", layeredModel.Children[5]);
            layeredModel.Metadata.Add($"{VolumeFace.Left}1", layeredModel.Children[6]);
            layeredModel.Metadata.Add($"{VolumeFace.Left}2", layeredModel.Children[7]);
            layeredModel.Metadata.Add($"{VolumeFace.Top}1", layeredModel.Children[8]);
            layeredModel.Metadata.Add($"{VolumeFace.Top}2", layeredModel.Children[9]);
            layeredModel.Metadata.Add($"{VolumeFace.Bottom}1", layeredModel.Children[10]);
            layeredModel.Metadata.Add($"{VolumeFace.Bottom}2", layeredModel.Children[11]);

            models.Add(layeredModel);
            return layeredModel;
        }

        public void ApplyTexture(LayeredModel3D layeredModel, VolumeFace face, string filename, TileMode tileMode = TileMode.Tile, Stretch stretch = Stretch.Uniform, double ratioX = 1, double ratioY = 1)
        {
            var metadata = (VolumeMetadata)layeredModel.Metadata[ModelType.Volume.ToString()];
            var textureMetadata = new TextureMetadata { Name = filename, TileMode = tileMode, Stretch = stretch, RatioX = ratioX, RatioY = ratioY };

            switch (face)
            {
                case VolumeFace.Front:
                    metadata.TextureFront = textureMetadata;
                    break;
                case VolumeFace.Back:
                    metadata.TextureBack = textureMetadata;
                    break;
                case VolumeFace.Top:
                    metadata.TextureTop = textureMetadata;
                    break;
                case VolumeFace.Bottom:
                    metadata.TextureBottom = textureMetadata;
                    break;
                case VolumeFace.Left:
                    metadata.TextureLeft = textureMetadata;
                    break;
                case VolumeFace.Right:
                    metadata.TextureRight = textureMetadata;
                    break;
            }

            var material = CreateMaterial(filename, tileMode, stretch, ratioX, ratioY);
            ApplyTextureToVolume(layeredModel, $"{face}1", material, TextureCoordinates1);
            ApplyTextureToVolume(layeredModel, $"{face}2", material, TextureCoordinates2);
        }
    }
}
