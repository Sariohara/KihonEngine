using KihonEngine.GameEngine;
using KihonEngine.GameEngine.Graphics;
using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using KihonEngine.GameEngine.State;
using KihonEngine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace KihonEngine.Studio.Services
{
    public class CameraPositionService
    {
        private ILogService LogService
            => Container.Get<ILogService>();
        private IGameEngineController GameEngineController
            => Container.Get<IGameEngineController>();
        private IGameEngineState State
            => Container.Get<IGameEngineState>();
        private ICameraController CameraController
            => Container.Get<ICameraController>();

        public void MoveToFront()
        {
            var box = GetWorldBox();
            var x = box.X + box.SizeX / 2;
            var y = box.Y + box.SizeY / 2 * 3;
            var z = box.Z + box.SizeZ / 2 * 3;

            var position = new Point3D(x, y, z);
            var center = GetCenter(box);

            MoveCamera(position, center - position);
        }

        public void MoveToBack()
        {
            var box = GetWorldBox();
            var x = box.X + box.SizeX / 2;
            var y = box.Y + box.SizeY / 2 * 3;
            var z = box.Z - box.SizeZ / 2;

            var position = new Point3D(x, y, z);
            var center = GetCenter(box);

            MoveCamera(position, center - position);
        }

        public void MoveToLeftSide()
        {
            var box = GetWorldBox();
            var x = box.X - box.SizeX / 2;
            var y = box.Y + box.SizeY / 2 * 3;
            var z = box.Z + box.SizeZ / 2;

            var position = new Point3D(x, y, z);
            var center = GetCenter(box);
            MoveCamera(position, center - position);
        }

        public void MoveToRightSide()
        {
            var box = GetWorldBox();
            var x = box.X + box.SizeX / 2 * 3;
            var y = box.Y + box.SizeY / 2 * 3;
            var z = box.Z + box.SizeZ / 2;

            var position = new Point3D(x, y, z);
            var center = GetCenter(box);
            MoveCamera(position, center - position);
        }

        public void MovetoRespawnPosition()
        {
            CameraController.Respawn();
            GameEngineController.NotifyIOs();
        }

        private void MoveCamera(Point3D position, Vector3D lookDirection)
        {
            lookDirection.Normalize();

            State.Graphics.PlayerCamera.Camera.Position = position;
            State.Graphics.PlayerCamera.Camera.LookDirection = lookDirection;

            State.Graphics.PlayerCamera.RotationXFromOrigin.Angle = 0;
            State.Graphics.PlayerCamera.RotationYFromOrigin.Angle = 0;
            State.Graphics.PlayerCamera.RotationZFromOrigin.Angle = 0;
            GameEngineController.NotifyIOs();
        }

        private Point3D GetCenter(Rect3D rect)
        {
            return new Point3D(rect.X + rect.SizeX / 2, rect.Y + rect.SizeY / 2, rect.Z + rect.SizeZ / 2);
        }

        private Rect3D GetWorldBox()
        {
            double xMin = 0;
            double yMin = 0;
            double zMin = 0;
            double xMax = 10;
            double yMax = 10;
            double zMax = 10;
            var firstWasChecked = false;
            foreach (var model in State.Graphics.Level)
            {
                if (model.Type != ModelType.Skybox && model.Type != ModelType.Light)
                {
                    Rect3D modelBounds = model.GetModel().Content.Bounds;

                    if (!firstWasChecked)
                    {
                        firstWasChecked = true;
                        xMin = modelBounds.X;
                        yMin = modelBounds.Y;
                        zMin = modelBounds.Z;
                        xMax = modelBounds.X + modelBounds.SizeX;
                        yMax = modelBounds.Y + modelBounds.SizeY;
                        zMax = modelBounds.Z + modelBounds.SizeZ;
                    }
                    else
                    {
                        if (modelBounds.X < xMin)
                        {
                            xMin = modelBounds.X;
                        }

                        if (modelBounds.Y < yMin)
                        {
                            yMin = modelBounds.Y;
                        }

                        if (modelBounds.Z < zMin)
                        {
                            zMin = modelBounds.Z;
                        }

                        if (modelBounds.X + modelBounds.SizeX > xMax)
                        {
                            xMax = modelBounds.X + modelBounds.SizeX;
                        }

                        if (modelBounds.Y + modelBounds.SizeY > yMax)
                        {
                            yMax = modelBounds.Y + modelBounds.SizeY;
                        }

                        if (modelBounds.Z + modelBounds.SizeZ > zMax)
                        {
                            zMax = modelBounds.Z + modelBounds.SizeZ;
                        }
                    }
                }
            }

            return new Rect3D(xMin, yMin, zMin, xMax - xMin, yMax - yMin, zMax - zMin);
        }
    }
}
