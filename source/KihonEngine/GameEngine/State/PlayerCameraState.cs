using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.State
{
    public class PlayerCameraState
    {
        public PerspectiveCamera Camera { get; set; }
        public AxisAngleRotation3D RotationXFromOrigin { get; set; }
        public AxisAngleRotation3D RotationYFromOrigin { get; set; }
        public AxisAngleRotation3D RotationZFromOrigin { get; set; }
    }
}
