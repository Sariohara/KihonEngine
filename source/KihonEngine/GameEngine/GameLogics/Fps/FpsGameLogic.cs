using System;
using System.Linq;
using System.Collections.Generic;
using KihonEngine.GameEngine.InputControls;
using System.Windows.Media.Media3D;
using KihonEngine.GameEngine.State.FpsGame;
using System.Threading;

namespace KihonEngine.GameEngine.GameLogics.Fps
{
    public class FpsGameLogic : GameLogicBase
    {
        public override string LogicName => "FPS GameLogic";

        private ICollisionManager _collisionManager;

        public FpsGameLogic()
        {
            _collisionManager = new CollisionManager();
        }

        private void CalculateMovesOfCameraOrientation(List<Action<GraphicUpdateContext>> graphicUpdates)
        {
            var mouseSettings = Configuration.GetMouseSettings();
            
            while (_mouseEvents.TryDequeue(out var mouseEvent))
            {
                if (mouseEvent.Type == MouseEventType.Move)
                {
                    if (mouseEvent.DX != 0)
                    {
                        graphicUpdates.Add(ctx => CameraController.RotateHorizontal(mouseEvent.DX * mouseSettings.MoveSpeed));
                    }

                    if (mouseEvent.DY != 0)
                    {
                        graphicUpdates.Add(ctx => CameraController.RotateVertical(-mouseEvent.DY * mouseSettings.MoveSpeed));
                    }
                }
                else
                {
                    // do nothing yet
                }
            }

            var keyboardSettings = Configuration.GetKeyboardSettings();
            var keys = _keyboardPressedKeys.ToArray();
            foreach (var key in keys)
            {
                if (key == keyboardSettings.LookRight)
                {
                    graphicUpdates.Add(ctx => CameraController.RotateHorizontal(keyboardSettings.MoveSpeed));
                }
                else if (key == keyboardSettings.LookLeft)
                {
                    graphicUpdates.Add(ctx => CameraController.RotateHorizontal(-keyboardSettings.MoveSpeed));
                }
                else if (key == keyboardSettings.LookUp)
                {
                    graphicUpdates.Add(ctx => CameraController.RotateVertical(keyboardSettings.MoveSpeed));
                }
                else if (key == keyboardSettings.LookDown)
                {
                    graphicUpdates.Add(ctx => CameraController.RotateVertical(-keyboardSettings.MoveSpeed));
                }
            }
        }

        private void LogCameraPositionChanged()
        {
            LogService.Log($"Moved to position ({State.Graphics.PlayerCamera.Camera.Position.X}, {State.Graphics.PlayerCamera.Camera.Position.Y}, {State.Graphics.PlayerCamera.Camera.Position.Z})");
        }

        private void DetectCollisionsAndMove(Point3D newPosition)
        {
            var playerBox = _collisionManager.GetPlayerBox(newPosition);
            var collisionResult = _collisionManager.DetectCollisions(playerBox);
            if (collisionResult.HasCollision)
            {
                if (collisionResult.HasBodyCollision || collisionResult.HasHeadCollision)
                {
                    return;
                }

                newPosition = new Point3D(newPosition.X, newPosition.Y + collisionResult.DeltaYForFeet, newPosition.Z);
            }
            else if (collisionResult.HasFeetInTheVoid && !State.Game.Get<JumpState>().IsJumping)
            {
                BeginToFall();
            }

            State.Graphics.PlayerCamera.Camera.Position = newPosition;
            LogCameraPositionChanged();
        }

        private void BeginToFall()
        {
            var keyboardSettings = Configuration.GetKeyboardSettings();
            var jumpState = State.Game.Reset<JumpState>();
            jumpState.IsJumping = true;
            jumpState.JumpDirection = State.Graphics.PlayerCamera.Camera.LookDirection;
            jumpState.HasMoveForward = _keyboardPressedKeys.Contains(keyboardSettings.MoveForward);
            jumpState.HasMoveBackward = _keyboardPressedKeys.Contains(keyboardSettings.MoveBackward);
            jumpState.HasMoveRight = _keyboardPressedKeys.Contains(keyboardSettings.MoveRight);
            jumpState.HasMoveLeft = _keyboardPressedKeys.Contains(keyboardSettings.MoveLeft);
            jumpState.YSpeed = -1;
        }

        public class GraphicUpdateContext
        {
            public Point3D NewPosition { get; set; }
        }

        private void SetNewPosition(GraphicUpdateContext ctx)
        {
            ctx.NewPosition = State.Graphics.PlayerCamera.Camera.Position;
        }

        private void BeginToJump(GraphicUpdateContext ctx)
        {
            var jumpState = State.Game.Reset<JumpState>();
            if (!jumpState.IsJumping)
            {
                var keyboardSettings = Configuration.GetKeyboardSettings();

                jumpState.IsJumping = true;
                jumpState.JumpDirection = State.Graphics.PlayerCamera.Camera.LookDirection;
                jumpState.HasMoveForward = _keyboardPressedKeys.Contains(keyboardSettings.MoveForward);
                jumpState.HasMoveBackward = _keyboardPressedKeys.Contains(keyboardSettings.MoveBackward);
                jumpState.HasMoveRight = _keyboardPressedKeys.Contains(keyboardSettings.MoveRight);
                jumpState.HasMoveLeft = _keyboardPressedKeys.Contains(keyboardSettings.MoveLeft);
                jumpState.YSpeed = jumpState.InitialYSpeed;
            }
        }

        private void PrepareMoveLongitudinal(GraphicUpdateContext ctx, double d)
        {
            if (State.Game.Get<JumpState>().IsJumping)
            {
                return;
            }

            var camera = State.Graphics.PlayerCamera.Camera;
            ctx.NewPosition = CameraController.GetMoveLongitudinal(ctx.NewPosition, camera.LookDirection, camera.UpDirection, d);
        }

        private void PrepareMoveLateral(GraphicUpdateContext ctx, double d)
        {
            if (State.Game.Get<JumpState>().IsJumping)
            {
                return;
            }

            var camera = State.Graphics.PlayerCamera.Camera;
            ctx.NewPosition = CameraController.GetMoveLateral(ctx.NewPosition, camera.LookDirection, camera.UpDirection, d);
        }

        private void DetectCollisionsAndMove(GraphicUpdateContext ctx)
        {
            var camera = State.Graphics.PlayerCamera.Camera;
            if (camera.Position != ctx.NewPosition)
            {
                DetectCollisionsAndMove(ctx.NewPosition);
            }
        }

        private void CalculateDamages(GraphicUpdateContext ctx)
        {
            var lifeState = State.Game.Get<LifeState>();
            var jumpState = State.Game.Get<JumpState>();
            if (!jumpState.IsJumping && jumpState.FallSize > jumpState.FallSizeLimitToDeath)
            {
                // Just end a jump, and jump was too heigh
                jumpState.IsJumping = false;
                lifeState.Life = 0;
            }

            if (!lifeState.IsAlive)
            {
                LogService.Log("Player is dead. Respawn.");
                
                State.Game.Reset<LifeState>();
                State.Game.Reset<JumpState>();

                CameraController.Respawn();
            }
        }

        private void CalculateJump(GraphicUpdateContext ctx)
        {
            var jumpState = State.Game.Get<JumpState>();
            if (!jumpState.IsJumping)
            {
                return;
            }

            var keyboardSettings = Configuration.GetKeyboardSettings();
            var camera = State.Graphics.PlayerCamera.Camera;
            var newPosition = camera.Position;
            if (jumpState.HasMoveForward)
            {
                newPosition = CameraController.GetMoveLongitudinal(newPosition, jumpState.JumpDirection, camera.UpDirection, keyboardSettings.MoveSpeed);
            }
            if (jumpState.HasMoveBackward)
            {
                newPosition = CameraController.GetMoveLongitudinal(newPosition, jumpState.JumpDirection, camera.UpDirection, -keyboardSettings.MoveSpeed);
            }
            if (jumpState.HasMoveRight)
            {
                newPosition = CameraController.GetMoveLateral(newPosition, jumpState.JumpDirection, camera.UpDirection, -keyboardSettings.MoveSpeed);
            }
            if (jumpState.HasMoveLeft)
            {
                newPosition = CameraController.GetMoveLateral(newPosition, jumpState.JumpDirection, camera.UpDirection, keyboardSettings.MoveSpeed);
            }

            jumpState.YSpeed -= State.Game.Get<JumpState>().Gravity;

            if (Math.Abs(jumpState.YSpeed) > jumpState.YSpeedMax)
            {
                var direction = jumpState.YSpeed >= 0? 1 : -1;
                jumpState.YSpeed = direction * jumpState.YSpeedMax;
            }

            if (jumpState.YSpeed < 0)
            {
                jumpState.FallSize += -jumpState.YSpeed;
            }

            newPosition = new Point3D(newPosition.X, newPosition.Y + jumpState.YSpeed, newPosition.Z);

            var playerBox = _collisionManager.GetPlayerBox(newPosition);
            var collisionResult = _collisionManager.DetectCollisions(playerBox);

            if (collisionResult.HasReachSkybox)
            {
                jumpState.YSpeed = 0;
                jumpState.IsJumping = false;
                
                return;
            }
            
            if (collisionResult.HasBodyCollision)
            {
                LogService.Log("Start body collision management");
                if (!newPosition.Equals(camera.Position))
                {
                    newPosition = camera.Position;
                    newPosition = new Point3D(newPosition.X, newPosition.Y + jumpState.YSpeed, newPosition.Z);
                    playerBox = _collisionManager.GetPlayerBox(newPosition);
                    collisionResult = _collisionManager.DetectCollisions(playerBox);
                }

                if (collisionResult.HasBodyCollision)
                {
                    var roundLimit = 100;
                    var round = 0;
                    var verticalDirection = 1;
                    if (jumpState.YSpeed < 0)
                    {
                        verticalDirection = -1;
                    }

                    while (collisionResult.HasBodyCollision)
                    {
                        LogService.Log($"Adapt position");
                        LogService.Log($"  Collision for position : {newPosition.X},{newPosition.Y},{newPosition.Z}");
                        LogService.Log($"  Collision increment    : { -verticalDirection}");
                        LogService.Log($"  Try new position       : {newPosition.X},{newPosition.Y - verticalDirection},{newPosition.Z}");
                        
                        newPosition = new Point3D(newPosition.X, newPosition.Y - verticalDirection, newPosition.Z);
                        playerBox = _collisionManager.GetPlayerBox(newPosition);
                        collisionResult = _collisionManager.DetectCollisions(playerBox);
                        LogService.Log($"  HasBodyCollision       : {collisionResult.HasBodyCollision}");

                        if (++round >= roundLimit && collisionResult.HasBodyCollision)
                        {
                            LogService.Log("==========  Player stuck in something ==========");
                            break;
                        }
                    }

                    jumpState.YSpeed = 0;
                }
            }

            if (collisionResult.HasHeadCollision)
            {
                newPosition = new Point3D(newPosition.X, collisionResult.AdjustedYForHead, newPosition.Z);
                jumpState.YSpeed = 0;
            }
            
            if (collisionResult.HasFeetCollision)
            {
                newPosition = new Point3D(newPosition.X, collisionResult.AdjustedYForFeet, newPosition.Z);
                jumpState.YSpeed = 0;
                jumpState.IsJumping = false;
            }

            if (collisionResult.HasFeetOnFloor)
            {
                jumpState.IsJumping = false;
            }

            if (newPosition != camera.Position)
            {
                camera.Position = newPosition;
                LogCameraPositionChanged();
            }
        }

        private void CalculateMovesOfPlayer(List<Action<GraphicUpdateContext>> graphicUpdates)
        {
            var keyboardSettings = Configuration.GetKeyboardSettings();
            var keys = _keyboardPressedKeys.ToArray();
            
            graphicUpdates.Add(ctx => SetNewPosition(ctx));
            foreach (var key in keys)
            {
                var jumpState = State.Game.Get<JumpState>();
                if (key == keyboardSettings.Jump)
                {
                    graphicUpdates.Add(ctx => BeginToJump(ctx));
                }
                else if (key == keyboardSettings.MoveForward)
                {
                    graphicUpdates.Add(ctx => PrepareMoveLongitudinal(ctx, keyboardSettings.MoveSpeed));
                }
                else if (key == keyboardSettings.MoveBackward)
                {
                    graphicUpdates.Add(ctx => PrepareMoveLongitudinal(ctx, -keyboardSettings.MoveSpeed));
                }
                else if (key == keyboardSettings.MoveRight)
                {
                    graphicUpdates.Add(ctx => PrepareMoveLateral(ctx, (-keyboardSettings.MoveSpeed)));
                }
                else if (key == keyboardSettings.MoveLeft)
                {
                    graphicUpdates.Add(ctx => PrepareMoveLateral(ctx, (keyboardSettings.MoveSpeed)));
                }
            }

            graphicUpdates.Add(ctx => DetectCollisionsAndMove(ctx));
            graphicUpdates.Add(ctx => CalculateJump(ctx));
            graphicUpdates.Add(ctx => CalculateDamages(ctx));
        }

        private void UpdateGraphics(List<Action<GraphicUpdateContext>> graphicUpdates)
        {
            if (graphicUpdates.Any())
            {
                var context = new GraphicUpdateContext();
                foreach (var update in graphicUpdates)
                {
                    State.Graphics.Viewport.Dispatcher.Invoke(() => update(context));
                }

                OnStateChanged();
            }
        }

        protected override void MainLoop(CancellationToken stoppingToken)
        {
            LogService.Log($"Game Started");

            State.Game.Reset();

            while (!stoppingToken.IsCancellationRequested)
            {
                Wait(10, stoppingToken);

                if (!stoppingToken.IsCancellationRequested)
                {
                    var graphicUpdates = new List<Action<GraphicUpdateContext>>();

                    // Calculate changes
                    CalculateMovesOfCameraOrientation(graphicUpdates);
                    CalculateMovesOfPlayer(graphicUpdates);

                    // Update graphics
                    UpdateGraphics(graphicUpdates);
                }
            }
            
            LogService.Log($"Game stopped");
        }
    }
}
