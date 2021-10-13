using KihonEngine.GameEngine.Graphics.ModelDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.Graphics.ModelsBuilders
{
    public class ModelBuilderFromDefinition
    {
        public LayeredModel3D Build(ModelBaseDefinition definition)
        {
            var level = new List<LayeredModel3D>();

            if (definition is CeilingDefinition)
            {
                BuildCeiling((CeilingDefinition)definition, level);
            }
            else if (definition is FloorDefinition)
            {
                BuildFloor((FloorDefinition)definition, level);
            }
            else if (definition is LightDefinition)
            {
                BuildLight((LightDefinition)definition, level);
            }
            else if (definition is SkyboxDefinition)
            {
                BuildSkybox((SkyboxDefinition)definition, level);
            }
            else if (definition is VolumeDefinition)
            {
                BuildVolume((VolumeDefinition)definition, level);
            }
            else if (definition is WallDefinition)
            {
                BuildWall((WallDefinition)definition, level);
            }
            else
            {
                throw new NotSupportedException($"Definition of type {definition.GetType().Name} are not supported");
            }

            return level.First();
        }

        private void BuildCeiling(CeilingDefinition definition, List<LayeredModel3D> level)
        {
            var builder = new CeilingBuilder(definition.Color, level);
            builder.UseBackMaterial = definition.Metadata.UseBackMaterial;
            var model = builder.Create((int)definition.Position.X, (int)definition.Position.Y, (int)definition.Position.Z, definition.Metadata.XSize, definition.Metadata.ZSize);
            if (!string.IsNullOrEmpty(definition.Metadata.Texture?.Name))
            {
                var texture = definition.Metadata.Texture;
                builder.ApplyTexture(model, texture.Name, texture.TileMode, texture.Stretch, texture.RatioX, texture.RatioY);
            }

            model.RotateByAxisX(definition.RotationAxisX);
            model.RotateByAxisY(definition.RotationAxisY);
            model.RotateByAxisZ(definition.RotationAxisZ);
        }

        private void BuildFloor(FloorDefinition definition, List<LayeredModel3D> level)
        {
            var builder = new FloorBuilder(definition.Color, level);
            builder.UseBackMaterial = definition.Metadata.UseBackMaterial;
            var model = builder.Create(definition.Position.X, definition.Position.Y, definition.Position.Z, definition.Metadata.XSize, definition.Metadata.ZSize);
            if (!string.IsNullOrEmpty(definition.Metadata.Texture?.Name))
            {
                var texture = definition.Metadata.Texture;
                builder.ApplyTexture(model, texture.Name, texture.TileMode, texture.Stretch, texture.RatioX, texture.RatioY);
            }

            model.RotateByAxisX(definition.RotationAxisX);
            model.RotateByAxisY(definition.RotationAxisY);
            model.RotateByAxisZ(definition.RotationAxisZ);
        }

        private void BuildLight(LightDefinition definition, List<LayeredModel3D> level)
        {
            var builder = new LightBuilder(definition.Color, level);
            builder.Create(definition.Metadata.Direction);
        }

        private void BuildSkybox(SkyboxDefinition definition, List<LayeredModel3D> level)
        {
            var builder = new SkyboxBuilder(definition.Color, level);
            builder.UseBackMaterial = definition.Metadata.UseBackMaterial;
            var model = builder.Create((int)definition.Position.X, (int)definition.Position.Y, (int)definition.Position.Z, definition.Metadata.Size, definition.Metadata.Normal, definition.Metadata.Name);
            model.RotateByAxisX(definition.RotationAxisX);
            model.RotateByAxisY(definition.RotationAxisY);
            model.RotateByAxisZ(definition.RotationAxisZ);
        }

        private void ApplyVolumeTexture(VolumeBuilder builder, LayeredModel3D model, VolumeFace face, TextureMetadata texture)
        {
            if (!string.IsNullOrEmpty(texture?.Name))
            {
                builder.ApplyTexture(model, face, texture.Name, texture.TileMode, texture.Stretch, texture.RatioX, texture.RatioY);
            }
        }

        private void BuildVolume(VolumeDefinition definition, List<LayeredModel3D> level)
        {
            var builder = new VolumeBuilder(definition.Color, level);
            builder.UseBackMaterial = definition.Metadata.UseBackMaterial;
            LayeredModel3D model = null;
            if (definition.Metadata.Opacity.HasValue)
            {
                var materialGroup = new MaterialGroup();
                var brush = new SolidColorBrush(definition.Color);
                brush.Opacity = definition.Metadata.Opacity.Value;
                materialGroup.Children.Add(new DiffuseMaterial(brush));

                model = builder.Create(definition.Position.X, definition.Position.Y, definition.Position.Z, definition.Metadata.XSize, definition.Metadata.YSize, definition.Metadata.ZSize, materialGroup);
            }
            else
            {
                model = builder.Create(definition.Position.X, definition.Position.Y, definition.Position.Z, definition.Metadata.XSize, definition.Metadata.YSize, definition.Metadata.ZSize);
            }

            ApplyVolumeTexture(builder, model, VolumeFace.Front, definition.Metadata.TextureFront);
            ApplyVolumeTexture(builder, model, VolumeFace.Back, definition.Metadata.TextureBack);
            ApplyVolumeTexture(builder, model, VolumeFace.Top, definition.Metadata.TextureTop);
            ApplyVolumeTexture(builder, model, VolumeFace.Bottom, definition.Metadata.TextureBottom);
            ApplyVolumeTexture(builder, model, VolumeFace.Left, definition.Metadata.TextureLeft);
            ApplyVolumeTexture(builder, model, VolumeFace.Right, definition.Metadata.TextureRight);

            model.RotateByAxisX(definition.RotationAxisX);
            model.RotateByAxisY(definition.RotationAxisY);
            model.RotateByAxisZ(definition.RotationAxisZ);
        }

        private void BuildWall(WallDefinition definition, List<LayeredModel3D> level)
        {
            var builder = new WallBuilder(definition.Color, level);
            builder.UseBackMaterial = definition.Metadata.UseBackMaterial;
            var model = builder.Create((int)definition.Position.X, (int)definition.Position.Y, (int)definition.Position.Z, (int)definition.Metadata.XSize, (int)definition.Metadata.YSize);
            if (!string.IsNullOrEmpty(definition.Metadata.Texture?.Name))
            {
                var texture = definition.Metadata.Texture;
                builder.ApplyTexture(model, texture.Name, texture.TileMode, texture.Stretch, texture.RatioX, texture.RatioY);
            }

            model.RotateByAxisX(definition.RotationAxisX);
            model.RotateByAxisY(definition.RotationAxisY);
            model.RotateByAxisZ(definition.RotationAxisZ);
        }
    }
}
