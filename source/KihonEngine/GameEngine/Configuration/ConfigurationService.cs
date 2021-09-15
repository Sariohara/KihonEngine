using KihonEngine.GameEngine.InputControls;
using System.Windows.Input;

namespace KihonEngine.GameEngine.Configuration
{
    

    public class ConfigurationService : IConfigurationService
    {
        public MouseSettings GetMouseSettings()
        {
            return new MouseSettings
            {
                MoveSpeed = 10,
            };
        }

        public KeyboardSettings GetKeyboardSettings()
        {
            return new KeyboardSettings
            {
                // Global Control
                CancelOperation = Key.Escape,
                FullScreenMode = Key.F11,

                // Editor Controls
                EditorMoveModelFromAxisX = Key.X,
                EditorMoveModelFromAxisY = Key.Y,
                EditorMoveModelFromAxisZ = Key.Z,

                // Game Controls
                LookRight = Key.G,
                LookLeft = Key.Q,
                LookUp = Key.R,
                LookDown = Key.V,

                Jump = Key.Space,
                MoveForward = Key.E,
                MoveBackward = Key.D,
                MoveRight = Key.F,
                MoveLeft = Key.S,

                MoveSpeed = 20,
            };
        }
    }
}
