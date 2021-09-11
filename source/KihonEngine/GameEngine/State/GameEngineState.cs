using KihonEngine.GameEngine.State.Editor;
using System.Windows.Media;

namespace KihonEngine.GameEngine.State
{
    public class GameEngineState : IGameEngineState
    {
        public GameEngineState()
        {
            Editor = new EditorState { CurrentColor = Colors.LightGray };
            Game = new GameState();
            Graphics = new State.GraphicState();
            
            EngineMode = EngineMode.PlayMode;
        }

        public GraphicState Graphics { get; set; }

        public EditorState Editor { get; set; }

        public GameState Game { get; set; }

        public EngineMode EngineMode { get; set; }

        public string CurrentLogicName { get; set; }

        public bool CanHandleMouseMoves { get; set; }
    }
}
