using KihonEngine.GameEngine.State;
using KihonEngine.Services;
using System;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.GameLogics.Fps
{
    public class CollisionManager : ICollisionManager
    {
        protected ILogService LogService { get; set; }
        protected IGameEngineState State { get; set; }

        public CollisionManager()
        {
            LogService = Container.Get<ILogService>();
            State = Container.Get<IGameEngineState>();
        }

        private const int PlayerCameraSizeY = 10;
        private const int PlayerSizeY = 12;
        private const int PlayerSizeX = 6;
        private const int PlayerSizeZ = 6;

        public Rect3D GetPlayerBox(Point3D cameraPosition)
        {
            var originX = cameraPosition.X - PlayerSizeX / 2;
            var originY = cameraPosition.Y - PlayerCameraSizeY;
            var originZ = cameraPosition.Z - PlayerSizeZ / 2;

            return new Rect3D(originX, originY, originZ, PlayerSizeX, PlayerSizeY, PlayerSizeZ);
        }

        public CollisionResult DetectCollisions(Rect3D animatedModelBox)
        {
            var result = new CollisionResult();

            foreach (var levelElement in State.Graphics.Level)
            {
                if (levelElement.Type == Graphics.ModelsBuilders.ModelType.Light)
                {
                    continue;
                }

                var box = new Rect3D(animatedModelBox.Location, animatedModelBox.Size);

                Rect3D levelElementBox = levelElement.GetModel().Content.Bounds;
                if (box.IntersectsWith(levelElementBox))
                {
                    box.Intersect(levelElementBox);

                    if (levelElement.Type == Graphics.ModelsBuilders.ModelType.Skybox)
                    {
                        if (
                            Math.Round(box.SizeY, 10) != Math.Round(animatedModelBox.SizeY, 10) 
                            || Math.Round(box.SizeX, 10) != Math.Round(animatedModelBox.SizeX, 10) ||
                            Math.Round(box.SizeZ, 10) != Math.Round(animatedModelBox.SizeZ, 10))
                        {
                            // Outside skybox
                            LogService.Log($"collision:reach skybox borders {levelElement.Type}:{box.X},{box.Y},{box.Z}:{box.SizeX},{box.SizeY},{box.SizeZ}");
                            result.HasCollision = true;
                            result.HasReachSkybox = true;
                            return result;
                        }

                        continue;
                    }

                    if (box.Y == animatedModelBox.Y && box.SizeY == 0)
                    {
                        LogService.Log($"collision:walk on {levelElement.Type}:{box.X},{box.Y},{box.Z}:{box.SizeX},{box.SizeY},{box.SizeZ}");
                        // Walk on something
                        result.HasFeetOnFloor = true;
                    }
                    else if (box.Y <= animatedModelBox.Y + 2 && box.Y + box.SizeY <= animatedModelBox.Y + 2)
                    {
                        LogService.Log($"collision:FeetCollision {levelElement.Type}:{box.X},{box.Y},{box.Z}:{box.SizeX},{box.SizeY},{box.SizeZ}");
                        // possible stairs
                        result.HasCollision = true;
                        result.HasFeetCollision = true;

                        if (box.SizeY >= result.DeltaYForFeet)
                        {
                            result.DeltaYForFeet = box.SizeY;
                            result.AdjustedYForFeet = animatedModelBox.Y + box.SizeY + PlayerCameraSizeY;
                        }
                    }
                    else if (box.Y >= animatedModelBox.Y + animatedModelBox.SizeY - 2)
                    {
                        LogService.Log($"collision:HeadCollision {levelElement.Type}:{box.X},{box.Y},{box.Z}:{box.SizeX},{box.SizeY},{box.SizeZ}");
                        // possible ceiling
                        result.HasCollision = true;
                        result.HasHeadCollision = true;

                        var delta = animatedModelBox.Y + animatedModelBox.SizeY - box.Y;
                        if (delta >= result.DeltaYForHead)
                        {
                            result.DeltaYForHead = delta;
                            result.AdjustedYForHead = box.Y - PlayerSizeY + PlayerCameraSizeY;
                        }
                    }
                    else
                    {
                        LogService.Log($"collision:BodyCollision with {levelElement.Type}:{box.X},{box.Y},{box.Z}:{box.SizeX},{box.SizeY},{box.SizeZ}");
                        result.HasCollision = true;
                        result.HasBodyCollision = true;
                        return result;
                    }
                }
            }

            result.HasFeetInTheVoid = !result.HasFeetCollision && !result.HasFeetOnFloor;

            LogService.Log($"inTheVoid:{result.HasFeetInTheVoid}");

            return result;
        }
    }
}
