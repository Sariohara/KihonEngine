using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.Graphics.ModelDefinitions
{
    public class ModelDefinitionBuilder
    {
        public MapDefinition CreateMapDefinition(string name, Point3D playerPosition, Vector3D playerLookDirection, List<LayeredModel3D> level)
        {
            var map = new MapDefinition
            {
                Name = name,
                PlayerPosition = playerPosition,
                PlayerLookDirection = playerLookDirection,
                Ceilings = new List<CeilingDefinition>(),
                Floors = new List<FloorDefinition>(),
                Lights = new List<LightDefinition>(),
                Skyboxes = new List<SkyboxDefinition>(),
                Volumes = new List<VolumeDefinition>(),
                Walls = new List<WallDefinition>(),
            };

            int index = 0;
            foreach (var layeredModel in level)
            {
                var definition = CreateModelDefinition(layeredModel);
                definition.Index = index++;

                if (definition is CeilingDefinition)
                {
                    map.Ceilings.Add((CeilingDefinition)definition);
                }
                else if (definition is FloorDefinition)
                {
                    map.Floors.Add((FloorDefinition)definition);
                }
                else if (definition is LightDefinition)
                {
                    map.Lights.Add((LightDefinition)definition);
                }
                else if (definition is SkyboxDefinition)
                {
                    map.Skyboxes.Add((SkyboxDefinition)definition);
                }
                else if (definition is VolumeDefinition)
                {
                    map.Volumes.Add((VolumeDefinition)definition);
                }
                else if (definition is WallDefinition)
                {
                    map.Walls.Add((WallDefinition)definition);
                }
            }

            return map;
        }

        public ModelBaseDefinition CreateModelDefinition(LayeredModel3D layeredModel3D)
        {
            ModelBaseDefinition definition = null;

            if (!layeredModel3D.Metadata.ContainsKey(layeredModel3D.Type.ToString()))
            {
                throw new FormatException($"Invalid layered model structure. Metadata entry <{layeredModel3D.Type}> not found.");
            }

            var layerMetadata = layeredModel3D.Metadata[layeredModel3D.Type.ToString()];
            if (layeredModel3D.Type == ModelType.Ceiling)
            {
                var metadata = layerMetadata as CeilingMetadata;
                definition = new CeilingDefinition();
                ((CeilingDefinition)definition).Metadata = metadata;
            }
            else if (layeredModel3D.Type == ModelType.Floor)
            {
                var metadata = layerMetadata as FloorMetadata;
                definition = new FloorDefinition();
                ((FloorDefinition)definition).Metadata = metadata;
            }
            else if (layeredModel3D.Type == ModelType.Light)
            {
                var metadata = layerMetadata as LightMetadata;
                definition = new LightDefinition();
                ((LightDefinition)definition).Metadata = metadata;
            }
            else if (layeredModel3D.Type == ModelType.Skybox)
            {
                var metadata = layerMetadata as SkyboxMetadata;
                definition = new SkyboxDefinition();
                ((SkyboxDefinition)definition).Metadata = metadata;
            }
            else if (layeredModel3D.Type == ModelType.Volume)
            {
                var metadata = layerMetadata as VolumeMetadata;
                definition = new VolumeDefinition();
                ((VolumeDefinition)definition).Metadata = metadata;
            }
            else if (layeredModel3D.Type == ModelType.Wall)
            {
                var metadata = layerMetadata as WallMetadata;
                definition = new WallDefinition();
                ((WallDefinition)definition).Metadata = metadata;
            }
            else
            {
                throw new NotSupportedException($"Not supported model type {layeredModel3D.Type}");
            }

            if (layeredModel3D.Type != ModelType.Light)
            {
                var translationMatrix = layeredModel3D.Translation.Value;
                definition.Position = new Point3D(translationMatrix.OffsetX, translationMatrix.OffsetY, translationMatrix.OffsetZ);
                definition.RotationAxisX = layeredModel3D.AxisXRotationAngle;
                definition.RotationAxisY = layeredModel3D.AxisYRotationAngle;
                definition.RotationAxisZ = layeredModel3D.AxisZRotationAngle;
            }

            definition.Color = layeredModel3D.GetColor();

            return definition;
        }
    }
}
