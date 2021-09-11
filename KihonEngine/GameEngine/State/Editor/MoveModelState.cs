
using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using KihonEngine.Studio.GameEngine.State.Editor;

namespace KihonEngine.GameEngine.State.Editor
{
    public class MoveModelState
    {
        public bool ShowMeterWallWhenMove { get; set; }
        public MoveModelMode Mode { get; set; }
        public double Delta { get; set; }
        public LayeredModel3D InvisibleMeterModel { get; set; }
    }
}
