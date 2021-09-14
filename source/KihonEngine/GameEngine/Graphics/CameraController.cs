using KihonEngine.GameEngine.State;
using KihonEngine.Services;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.Graphics
{
    public class CameraController : ICameraController
    {
        private ILogService LogService
            => Container.Get<ILogService>();
        private IGameEngineState State
            => Container.Get<IGameEngineState>();

        public void Respawn()
        {
            var camera = State.Graphics.PlayerCamera.Camera;
            camera.UpDirection = new Vector3D(0, 1, 0);
            camera.Position = State.Graphics.RespawnPosition;
            camera.LookDirection = State.Graphics.RespawnLookDirection;

            State.Graphics.PlayerCamera.RotationXFromOrigin.Angle = 0;
            State.Graphics.PlayerCamera.RotationYFromOrigin.Angle = 0;
            State.Graphics.PlayerCamera.RotationZFromOrigin.Angle = 0;
        }

        public void MoveLongitudinal(double d)
        {
            double u = 0.05;
            PerspectiveCamera camera = State.Graphics.PlayerCamera.Camera;
            
            Vector3D direction = new Vector3D(camera.LookDirection.X, 0, camera.LookDirection.Z);
            direction.Normalize();

            camera.Position = camera.Position + u * direction * d;
            LogCameraPositionChanged();
        }

        public void MoveVertical(double d)
        {
            double u = 0.05;
            PerspectiveCamera camera = State.Graphics.PlayerCamera.Camera;

            Vector3D direction = camera.UpDirection;
            direction.Normalize();

            camera.Position = camera.Position + u * direction * d;
            LogCameraPositionChanged();
        }

        public void MoveLateral(double d)
        {
            double u = 0.05;
            PerspectiveCamera camera = State.Graphics.PlayerCamera.Camera;

            var direction = Vector3D.CrossProduct(camera.UpDirection, camera.LookDirection);
            direction.Normalize();

            camera.Position = camera.Position + u * direction * d;
            LogCameraPositionChanged();
        }

        public void RotateHorizontal(double d)
        {
            double u = 0.05;
            double angleD = u * d;
            PerspectiveCamera camera = State.Graphics.PlayerCamera.Camera;

            var m = new Matrix3D();
            m.Rotate(new Quaternion(camera.UpDirection, -angleD)); // Rotate about the camera's up direction to look left/right
            camera.LookDirection = m.Transform(camera.LookDirection);

            LogLookDirectionChanged();
        }

        public void RotateVertical(double d)
        {
            double u = 0.05;
            double angleD = u * d;
            PerspectiveCamera camera = State.Graphics.PlayerCamera.Camera;

            // Cross Product gets a vector that is perpendicular to the passed in vectors (order does matter, reverse the order and the vector will point in the reverse direction)
            var cp = Vector3D.CrossProduct(camera.UpDirection, camera.LookDirection);
            cp.Normalize();

            var m = new Matrix3D();
            m.Rotate(new Quaternion(cp, -angleD)); // Rotate about the vector from the cross product
            camera.LookDirection = m.Transform(camera.LookDirection);

            LogLookDirectionChanged();
        }

        public void RotateFromOriginOnAxisX(double angle)
        {
            State.Graphics.PlayerCamera.RotationXFromOrigin.Angle = angle;
            LogLookDirectionChanged();
        }

        public void DeltaRotateFromOriginOnAxisX(double angle)
        {
            State.Graphics.PlayerCamera.RotationXFromOrigin.Angle += angle;
            LogLookDirectionChanged();
        }

        public void RotateFromOriginOnAxisY(double angle)
        {
            State.Graphics.PlayerCamera.RotationYFromOrigin.Angle = angle;
            LogLookDirectionChanged();
        }

        public void DeltaRotateFromOriginOnAxisY(double angle)
        {
            State.Graphics.PlayerCamera.RotationYFromOrigin.Angle += angle;
            LogLookDirectionChanged();
        }

        public void RotateFromOriginOnAxisZ(double angle)
        {
            State.Graphics.PlayerCamera.RotationZFromOrigin.Angle = angle;
            LogLookDirectionChanged();
        }

        public void DeltaRotateFromOriginOnAxisZ(double angle)
        {
            State.Graphics.PlayerCamera.RotationZFromOrigin.Angle += angle;
            LogLookDirectionChanged();
        }

        public void RotateDZ(double angle)
        {
            State.Graphics.PlayerCamera.RotationZFromOrigin.Angle += angle;
            LogLookDirectionChanged();
        }

        public void ZoomFromOrigin(double newValue)
        {
            State.Graphics.PlayerCamera.Camera.Position = new Point3D(State.Graphics.PlayerCamera.Camera.Position.X, State.Graphics.PlayerCamera.Camera.Position.Y, newValue);
            LogLookDirectionChanged();
        }

        public void DeltaZoomFromOrigin(double newValue)
        {
            State.Graphics.PlayerCamera.Camera.Position = new Point3D(State.Graphics.PlayerCamera.Camera.Position.X, State.Graphics.PlayerCamera.Camera.Position.Y, State.Graphics.PlayerCamera.Camera.Position.Z + newValue);
            LogLookDirectionChanged();
        }

        private void LogCameraPositionChanged()
        {
            LogService.Log($"Moved to position ({State.Graphics.PlayerCamera.Camera.Position.X}, {State.Graphics.PlayerCamera.Camera.Position.Y}, {State.Graphics.PlayerCamera.Camera.Position.Z})");
        }

        private void LogLookDirectionChanged()
        {
            LogService.Log($"Changed LookDirection ({State.Graphics.PlayerCamera.Camera.LookDirection.X}, {State.Graphics.PlayerCamera.Camera.LookDirection.Y}, {State.Graphics.PlayerCamera.Camera.LookDirection.Z})");
        }
    }
}
