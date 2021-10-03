using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.State
{
    public class PlayerCameraState
    {
        public PerspectiveCamera Camera { get; set; }
        public AxisAngleRotation3D RotationXFromOrigin { get; set; }
        public AxisAngleRotation3D RotationYFromOrigin { get; set; }
        public AxisAngleRotation3D RotationZFromOrigin { get; set; }

        public Point3D GetTransformedPosition()
        {
            var position = Camera.Position;
            position = new RotateTransform3D(RotationXFromOrigin).Transform(position);
            position = new RotateTransform3D(RotationYFromOrigin).Transform(position);
            position = new RotateTransform3D(RotationZFromOrigin).Transform(position);

            return position;
        }
    }
}
