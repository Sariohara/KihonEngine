using KihonEngine.GameEngine.Graphics.Maps;
using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using KihonEngine.GameEngine.State;
using KihonEngine.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.Graphics
{
    public class WorldEngine : IWorldEngine
    {
        private ILogService LogService
            => Container.Get<ILogService>();
        private IGameEngineState State
            => Container.Get<IGameEngineState>();
        private IMapBuilderFactory MapBuilderFactory
            => Container.Get<IMapBuilderFactory>();

        public string[] RegisteredMaps => MapBuilderFactory.GetRegisteredMaps();

        public void RegisterMap(IMapBuilder mapBuilder)
        {
            MapBuilderFactory.RegisterMap(mapBuilder);
        }

        public string RegisterMap<TMapBuilder>() where TMapBuilder : class, IMapBuilder, new()
        {
            var mapBuilder = new TMapBuilder();
            MapBuilderFactory.RegisterMap(mapBuilder);

            return mapBuilder.MapName;
        }

        public void LoadMap(string mapName)
        {
            LoadMap(MapBuilderFactory.Get(mapName));
        }

        public void LoadMap(IMapBuilder mapBuilder)
        {
            // Create level
            State.Graphics.Level = mapBuilder.CreateMap();
            State.Graphics.LevelName = mapBuilder.MapName;

            // Set camera position on the map
            State.Graphics.RespawnPosition = mapBuilder.PlayerPosition;
            State.Graphics.RespawnLookDirection = mapBuilder.PlayerLookDirection;
            State.Graphics.PlayerCamera = InitializeCamera();
            State.Graphics.PlayerCamera.Camera.LookDirection = mapBuilder.PlayerLookDirection;
            State.Graphics.PlayerCamera.Camera.Position = mapBuilder.PlayerPosition;

            BindMapToViewport();
        }

        private void BindMapToViewport()
        {
            // Ensure viewport is created
            if (State.Graphics.Viewport == null)
            {
                State.Graphics.Viewport = new Viewport3D() { ClipToBounds = true, Focusable = true };
            }

            // Attach world
            State.Graphics.Viewport.Children.Clear();

            foreach (var model in State.Graphics.Level)
            {
                State.Graphics.Viewport.Children.Add(model.GetModel());
            }

            // Attach camera
            State.Graphics.Viewport.Camera = State.Graphics.PlayerCamera.Camera;
            var transform3dGroup = new Transform3DGroup();
            transform3dGroup.Children.Add(new RotateTransform3D(State.Graphics.PlayerCamera.RotationYFromOrigin));
            transform3dGroup.Children.Add(new RotateTransform3D(State.Graphics.PlayerCamera.RotationXFromOrigin));
            transform3dGroup.Children.Add(new RotateTransform3D(State.Graphics.PlayerCamera.RotationZFromOrigin));
            State.Graphics.Viewport.Camera.Transform = transform3dGroup;
        }

        public LayeredModel3D GetModel(Point position)
        {
            return GetModel(position, out var hitPoint, out var modelHit);
        }

        public LayeredModel3D GetModel(Point position, out Point3D? hitPoint)
        {
            return GetModel(position, out hitPoint, out var modelHit);
        }

        public LayeredModel3D GetModel(Point position, out Point3D? hitPoint, out Model3D modelHit)
        {
            hitPoint = null;
            modelHit = null;

            // Perform the hit test
            HitTestResult result = VisualTreeHelper.HitTest(State.Graphics.Viewport, position);
            
            // Display information about the hit
            RayMeshGeometry3DHitTestResult meshResult = result as RayMeshGeometry3DHitTestResult;

            if (meshResult == null)
            {
                LogService.Log("nothing here");
                return null;
            }
            else
            {
                hitPoint = meshResult.PointHit;
                modelHit = meshResult.ModelHit;

                var layeredModel = GetLayeredModel3D(meshResult.ModelHit);
                if (layeredModel != null)
                {
                    LogService.Log("Linked to a layered model");
                }
                else
                {
                    LogService.Log("Not linked to a layered model");
                }

                // Display more detail about the hit.
                var cameraPosition = State.Graphics.PlayerCamera.Camera.Position;
                double deltaX = meshResult.PointHit.X - cameraPosition.X;
                double deltaY = meshResult.PointHit.Y - cameraPosition.Y;
                double deltaZ = meshResult.PointHit.Z - cameraPosition.Z;
                //meshResult.
                //float distance = (float)System.Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);

                //LogService.Log("Distance from camera: " + distance);
                LogService.Log("Distance: " + meshResult.DistanceToRayOrigin);
                LogService.Log("Point hit: (" + meshResult.PointHit.ToString() + ")");

                LogService.Log("Triangle:");
                MeshGeometry3D mesh = meshResult.MeshHit;
                LogService.Log("    (" + mesh.Positions[meshResult.VertexIndex1].ToString() + ")");
                LogService.Log("    (" + mesh.Positions[meshResult.VertexIndex2].ToString() + ")");
                LogService.Log("    (" + mesh.Positions[meshResult.VertexIndex3].ToString() + ")");

                return layeredModel;
            }
        }        

        public void AddModel(LayeredModel3D model)
        {
            if (model != null)
            {
                State.Graphics.Level.Add(model);
                State.Graphics.Viewport.Children.Add(model.GetModel());
            }
        }

        public void RemoveModel(LayeredModel3D model)
        {
            if (model != null)
            {
                State.Graphics.Level.Remove(model);
                State.Graphics.Viewport.Children.Remove(model.GetModel());
            }
        }

        public void ReplaceModel(LayeredModel3D oldModel, LayeredModel3D newModel)
        {
            if (oldModel == null || newModel == null)
            {
                throw new ArgumentNullException("oldModel or newModel is null");
            }
            
            var index = State.Graphics.Level.IndexOf(oldModel);

            State.Graphics.Level.Remove(oldModel);
            State.Graphics.Viewport.Children.Remove(oldModel.GetModel());

            State.Graphics.Level.Insert(index, newModel);
            State.Graphics.Viewport.Children.Add(newModel.GetModel());
        }

        private LayeredModel3D GetLayeredModel3D(Model3D modelHit)
        {
            foreach (var layeredModel in State.Graphics.Level)
            {
                foreach (var children in layeredModel.Children)
                {
                    if (children is Model3DGroup && ((Model3DGroup)children).Children.Contains(modelHit))
                    {
                        return layeredModel;
                    }
                }
            }

            return null;
        }

        private PlayerCameraState InitializeCamera()
        {
            var cameraContext = new PlayerCameraState();
            cameraContext.Camera = new PerspectiveCamera
            {
                UpDirection = new Vector3D(0, 1, 0),
                LookDirection = new Vector3D(0, 0, -1),
                Position = new Point3D(10, 10, 10),
                FieldOfView = 90,
                FarPlaneDistance = 20000,
                NearPlaneDistance = 1
            };

            cameraContext.RotationYFromOrigin = CreateRotation(new Vector3D(0, 1, 0), 0);
            cameraContext.RotationXFromOrigin = CreateRotation(new Vector3D(1, 0, 0), 0);
            cameraContext.RotationZFromOrigin = CreateRotation(new Vector3D(0, 0, 1), 0);

            return cameraContext;
        }

        private AxisAngleRotation3D CreateRotation(Vector3D axis, double angle)
        {
            AxisAngleRotation3D myAxisAngleRotation3d = new AxisAngleRotation3D();
            myAxisAngleRotation3d.Axis = axis;
            myAxisAngleRotation3d.Angle = angle;
            return myAxisAngleRotation3d;
        }
    }
}
