using System.Windows.Input;

namespace KihonEngine.GameEngine.InputControls
{
    public class KeyboardSettings
    {
        public Key CancelOperation { get; set; }
        public Key FullScreenMode { get; set; }

        public Key EditorMoveModelFromAxisX { get; set; }
        public Key EditorMoveModelFromAxisY { get; set; }
        public Key EditorMoveModelFromAxisZ { get; set; }
        public Key[] EditorMoveModelKeys => new[] { EditorMoveModelFromAxisX, EditorMoveModelFromAxisY, EditorMoveModelFromAxisZ };

        public Key LookRight { get; set; }
        public Key LookLeft { get; set; }
        public Key LookUp { get; set; }
        public Key LookDown { get; set; }
        public Key[] LookKeys => new[] { LookRight, LookLeft, LookUp, LookDown };

        public Key MoveForward { get; set; }
        public Key MoveBackward { get; set; }
        public Key MoveRight { get; set; }
        public Key MoveLeft { get; set; }
        public Key[] MoveKeys => new[] { MoveRight, MoveLeft, MoveForward, MoveBackward };

        public double MoveSpeed { get; set; }
    }
}
