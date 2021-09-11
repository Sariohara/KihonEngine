using System;
using System.Linq;
using System.Collections.Generic;
using KihonEngine.GameEngine.InputControls;

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
            var keyboardSettings = Configuration.GetKeyboardSettings();
            var keys = _keyboardPressedKeys.ToArray();
            foreach (var key in keys)
            {
                if (key == keyboardSettings.MoveForward)
                {
                    graphicUpdates.Add(() => CameraController.MoveLongitudinal(keyboardSettings.MoveSpeed));
                }
                else if (key == keyboardSettings.MoveBackward)
                {
                    graphicUpdates.Add(() => CameraController.MoveLongitudinal(-keyboardSettings.MoveSpeed));
                }
                else if (key == keyboardSettings.MoveRight)
                {
                    graphicUpdates.Add(() => CameraController.MoveLateral(-keyboardSettings.MoveSpeed));
                }
                else if (key == keyboardSettings.MoveLeft)
                {
                    graphicUpdates.Add(() => CameraController.MoveLateral(keyboardSettings.MoveSpeed));
                }
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

            while (!ShouldStop)
            {
                System.Threading.Thread.Sleep(10);

                var graphicUpdates = new List<Action>();

                // Calculate changes
                CalculateMovesOfCameraOrientation(graphicUpdates);
                CalculateMovesOfPlayer(graphicUpdates);

                // Update graphics
                UpdateGraphics(graphicUpdates);
            }

            LogService.Log($"Game stopped");
        }
    }
}
