
namespace KihonEngine.GameEngine.Graphics
{
    public interface ICameraController
    {
        void Respawn();
        void MoveLongitudinal(double d);
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
