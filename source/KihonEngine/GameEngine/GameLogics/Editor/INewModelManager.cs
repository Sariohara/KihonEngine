using KihonEngine.GameEngine.Graphics.ModelDefinitions;

namespace KihonEngine.GameEngine.GameLogics.Editor
{
    public interface INewModelManager
    {
        void StartAddNewModel(ModelBaseDefinition definition);
        bool UpdateNewModelPosition(System.Windows.Point mousePosition);
        void CancelAddNewModel();
        void ApplyNewModel();
    }
}
