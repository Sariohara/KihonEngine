using KihonEngine.GameEngine.Graphics.ModelDefinitions;
using KihonEngine.GameEngine.Graphics.ModelsBuilders;

namespace KihonEngine.GameEngine.State.Editor
{
    public class NewModelState
    {
        public NewModelMode Mode { get; set; }
        public LayeredModel3D NewModel { get; set; }
        public LayeredModel3D NewModelSelectionBox { get; set; }
        public LayeredModel3D InvisibleMeterModel { get; set; }
    }
}
