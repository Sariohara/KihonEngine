using KihonEngine.GameEngine.State;
using KihonEngine.Services;
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

        public Rect3D GetPlayerBox(Point3D cameraPosition)
        {
            var playerSizeX = 6;
            var playerSizeY = 12;
            var playerSizeZ = 6;
            var originX = cameraPosition.X - playerSizeX / 2;
            var originY = cameraPosition.Y - PlayerCameraSizeY;
            var originZ = cameraPosition.Z - playerSizeZ / 2;

            return new Rect3D(originX, originY, originZ, playerSizeX, playerSizeY, playerSizeZ);
        }

        public CollisionResult DetectCollisions(Rect3D animatedModelBox)
        {
            var result = new CollisionResult();

            foreach (var levelElement in State.Graphics.Level)
            {
                if (levelElement.Type == Graphics.ModelsBuilders.ModelType.Skybox
                    || levelElement.Type == Graphics.ModelsBuilders.ModelType.Light)
                {
                    continue;
                }

                var box = new Rect3D(animatedModelBox.Location, animatedModelBox.Size);

                Rect3D levelElementBox = levelElement.GetModel().Content.Bounds;
                if (box.IntersectsWith(levelElementBox))
                {
                    box.Intersect(levelElementBox);

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
                            result.AdjustedYForHead = animatedModelBox.Y - box.Y;
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
