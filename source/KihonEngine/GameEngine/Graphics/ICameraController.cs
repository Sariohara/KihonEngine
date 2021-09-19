
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.Graphics
{
    public interface ICameraController
    {
        void SetPosition(Point3D newPosition);
        void Respawn();
        Point3D GetMoveLongitudinal(Point3D position, Vector3D lookDirection, Vector3D upDirection, double d);
        Point3D GetMoveVertical(Point3D position, Vector3D lookDirection, Vector3D upDirection, double d);
        Point3D GetMoveLateral(Point3D position, Vector3D lookDirection, Vector3D upDirection, double d);
        void MoveLongitudinal(double d);
        void MoveVertical(double d);
        void MoveLateral(double d);
        public void RotateHorizontal(double d);
        public void RotateVertical(double d);

        void RotateFromOriginOnAxisX(double angle);        
        void RotateFromOriginOnAxisY(double angle);
        void RotateFromOriginOnAxisZ(double angle);
        void ZoomFromOrigin(double newValue);

        void DeltaRotateFromOriginOnAxisX(double angle);
        void DeltaRotateFromOriginOnAxisY(double angle);
        void DeltaRotateFromOriginOnAxisZ(double angle);
        void DeltaZoomFromOrigin(double newValue);
    }
}
