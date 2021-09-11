using System.Windows.Media;

namespace KihonEngine.GameEngine.State.Editor
{
    public class EditorState
    {
        public EditorState()
        {
            CurrentColor = Colors.Transparent;
            TranslationStep = 1;
            RotationStep = 1;
            ActionSelect = new SelectModelState();
            ActionMove = new MoveModelState();
        }

        public Color CurrentColor { get; set; }

        public SelectModelState ActionSelect { get; set; }

        public MoveModelState ActionMove { get; set; }

        public int TranslationStep { get; set; }

        public int RotationStep { get; set; }
    }
}
