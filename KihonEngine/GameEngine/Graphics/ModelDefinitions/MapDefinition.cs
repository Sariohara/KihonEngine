using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.Graphics.ModelDefinitions
{
    public class MapDefinition
    {
        public string Name { get; set; }
        public Point3D PlayerPosition { get; set; }
        public Vector3D PlayerLookDirection { get; set; }

        public List<CeilingDefinition> Ceilings { get; set; }
        public List<FloorDefinition> Floors { get; set; }
        public List<LightDefinition> Lights { get; set; }
        public List<SkyboxDefinition> Skyboxes { get; set; }
        public List<VolumeDefinition> Volumes { get; set; }
        public List<WallDefinition> Walls { get; set; }
    }
}
