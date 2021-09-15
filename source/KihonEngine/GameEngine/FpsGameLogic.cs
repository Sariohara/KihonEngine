using System;
using System.Linq;
using System.Collections.Generic;
using KihonEngine.GameEngine.InputControls;
using System.Windows.Media.Media3D;
using KihonEngine.GameEngine.State.FpsGame;
using KihonEngine.GameEngine.State;

namespace KihonEngine.GameEngine
{
    public class FpsGameLogic : GameLogicBase
    {
        public override string LogicName => "FPS GameLogic";

        public FpsGameLogic() : base()
        {

        }

        

        private void CalculateMovesOfCameraOrientation(List<Action> graphicUpdates)
        {
            var mouseSettings = Configuration.GetMouseSettings();
            
            while (_mouseEvents.TryDequeue(out var mouseEvent))
            {
                if (mouseEvent.Type == MouseEventType.Move)
                {
                    if (mouseEvent.DX != 0)
                    {
                        graphicUpdates.Add(() => CameraController.RotateHorizontal(mouseEvent.DX * mouseSettings.MoveSpeed));
                    }

                    if (mouseEvent.DY != 0)
                    {
                        graphicUpdates.Add(() => CameraController.RotateVertical(-mouseEvent.DY * mouseSettings.MoveSpeed));
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
                    graphicUpdates.Add(() => CameraController.RotateHorizontal(keyboardSettings.MoveSpeed));
                }
                else if (key == keyboardSettings.LookLeft)
                {
                    graphicUpdates.Add(() => CameraController.RotateHorizontal(-keyboardSettings.MoveSpeed));
                }
                else if (key == keyboardSettings.LookUp)
                {
                    graphicUpdates.Add(() => CameraController.RotateVertical(keyboardSettings.MoveSpeed));
                }
                else if (key == keyboardSettings.LookDown)
                {
                    graphicUpdates.Add(() => CameraController.RotateVertical(-keyboardSettings.MoveSpeed));
                }
            }
        }

        private void CalculateMovesOfPlayer(List<Action> graphicUpdates)
        {
            if (State.Game.Get<FallState>().IsFalling && !State.Game.Get<FallState>().CanMoveWhenFall)
            {
                return;
            }

            var keyboardSettings = Configuration.GetKeyboardSettings();
            var keys = _keyboardPressedKeys.ToArray();
            foreach (var key in keys)
            {
                var jumpState = State.Game.Get<JumpState>();
                if (key == keyboardSettings.Jump && !State.Game.Get<FallState>().IsFalling && !State.Game.Get<JumpState>().IsJumping)
                {
                    jumpState = State.Game.Reset<JumpState>();
                    jumpState.IsJumping = true;
                    jumpState.HasMoveForward = _keyboardPressedKeys.Contains(keyboardSettings.MoveForward);
                    jumpState.HasMoveBackward = _keyboardPressedKeys.Contains(keyboardSettings.MoveBackward);
                    jumpState.HasMoveRight = _keyboardPressedKeys.Contains(keyboardSettings.MoveRight);
                    jumpState.HasMoveLeft = _keyboardPressedKeys.Contains(keyboardSettings.MoveLeft);
                    jumpState.YSpeed = jumpState.InitialYSpeed;
                }
                else if (
                    key == keyboardSettings.MoveForward 
                    || (jumpState.IsJumping && jumpState.HasMoveForward))
                {
                    graphicUpdates.Add(() => CameraController.MoveLongitudinal(keyboardSettings.MoveSpeed, !jumpState.IsJumping));
                }
                else if (
                    key == keyboardSettings.MoveBackward
                    || (jumpState.IsJumping && jumpState.HasMoveBackward))
                {
                    graphicUpdates.Add(() => CameraController.MoveLongitudinal(-keyboardSettings.MoveSpeed, !jumpState.IsJumping));
                }
                else if (
                    key == keyboardSettings.MoveRight
                    || (jumpState.IsJumping && jumpState.HasMoveRight))
                {
                    graphicUpdates.Add(() => CameraController.MoveLateral(-keyboardSettings.MoveSpeed, !jumpState.IsJumping));
                }
                else if (
                    key == keyboardSettings.MoveLeft
                    || (jumpState.IsJumping && jumpState.HasMoveLeft))
                {
                    graphicUpdates.Add(() => CameraController.MoveLateral(keyboardSettings.MoveSpeed, !jumpState.IsJumping));
                }
            }
        }

        public void CalculateFalling(List<Action> graphicUpdates)
        {
            if(State.Game.Get<JumpState>().IsJumping && State.Game.Get<JumpState>().YSpeed > 0)
            {
                return;
            }

            graphicUpdates.Add(() =>
            {
                CameraController.HasCollisions(true, State.Graphics.PlayerCamera.Camera.Position, out var adjustmentY);
                var fallState = State.Game.Get<FallState>();
                if (adjustmentY < 0)
                {
                    fallState.IsFalling = true;
                    fallState.FallHeigh++;
                    fallState.CanMoveWhenFall = fallState.FallHeigh < 10;
                    fallState.DeadFall = fallState.FallHeigh > 50;

                    CameraController.MoveVertical(adjustmentY, true);
                }
                else
                {
                    fallState.IsFalling = false;
                    fallState.FallHeigh = 0;
                    fallState.CanMoveWhenFall = true;
                    fallState.DeadFall = false;
                }

                if (State.Game.Get<FallState>().DeadFall)
                {
                    CameraController.Respawn();
                }
            });
        }

        public void CalculateJump(List<Action> graphicUpdates)
        {
            if (State.Game.Get<JumpState>().IsJumping)
            {
                graphicUpdates.Add(() =>
                {
                    State.Game.Get<JumpState>().YSpeed -= State.Game.Get<JumpState>().Gravity;
                    State.Game.Get<JumpState>().IsJumping = State.Game.Get<JumpState>().YSpeed > -1;

                    var position = State.Graphics.PlayerCamera.Camera.Position;
                    double yMoveWithoutCollisions = 0;
                    for (double yMove = 0; yMove < State.Game.Get<JumpState>().YSpeed; yMove++)
                    {
                        var intermediateNewPosition = new Point3D(position.X, position.Y + yMove, position.Z);
                        if (!CameraController.HasCollisions(true, intermediateNewPosition, out var adjustmentY))
                        {
                            yMoveWithoutCollisions = yMove;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (yMoveWithoutCollisions != 0)
                    {
                        CameraController.MoveVertical(yMoveWithoutCollisions, false);
                    }
                    else
                    {
                        State.Game.Get<JumpState>().IsJumping = false;
                    }
                });
            }
        }

        private void UpdateGraphics(List<Action> graphicUpdates)
        {
            if (graphicUpdates.Any())
            {
                foreach (var update in graphicUpdates)
                {
                    State.Graphics.Viewport.Dispatcher.Invoke(update);
                }

                OnStateChanged();
            }
        }

        protected override void MainLoop()
        {
            LogService.Log($"Game Started");

            State.Game.Reset();

            while (!ShouldStop)
            {
                System.Threading.Thread.Sleep(10);

                var graphicUpdates = new List<Action>();

                // Calculate changes
                CalculateMovesOfCameraOrientation(graphicUpdates);
                CalculateMovesOfPlayer(graphicUpdates);
                CalculateJump(graphicUpdates);
                CalculateFalling(graphicUpdates);

                // Update graphics
                UpdateGraphics(graphicUpdates);
            }

            LogService.Log($"Game stopped");
        }
    }
}
