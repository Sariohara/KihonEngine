
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.Graphics
{
    public interface ICameraController
    {
        bool HasCollisions(bool useClipping, Point3D position, out double adjustmentY);

        void Respawn();
        void MoveLongitudinal(double d, bool useClipping);
        void MoveVertical(double d, bool useClipping);
        void MoveLateral(double d, bool useClipping);
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
