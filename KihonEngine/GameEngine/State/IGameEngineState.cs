
using KihonEngine.GameEngine.State.Editor;

namespace KihonEngine.GameEngine.State
{
    public interface IGameEngineState
    {
        public GraphicState Graphics { get; set; }

        public EditorState Editor { get; set; }

        public GameState Game { get; set; }

        public EngineMode EngineMode { get; set; }

        public string CurrentLogicName { get; set; }

        public bool CanHandleMouseMoves { get; set; }
    }
}
