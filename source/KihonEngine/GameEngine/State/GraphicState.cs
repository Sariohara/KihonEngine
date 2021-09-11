using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.State
{
    public class GraphicState
    {
        public GraphicState()
        {
            Level = new List<LayeredModel3D>();
            PlayerCamera = new PlayerCameraState();
        }

        public Viewport3D Viewport { get; set; }

        public string LevelName { get; set; }

        public List<LayeredModel3D> Level { get; set; }

        public Point3D RespawnPosition { get; set; }

        public Vector3D RespawnLookDirection { get; set; }

        public PlayerCameraState PlayerCamera { get; set; }
    }
}
