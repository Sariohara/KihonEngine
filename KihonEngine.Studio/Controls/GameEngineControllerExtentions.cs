
using KihonEngine.GameEngine;
using KihonEngine.GameEngine.Graphics;
using KihonEngine.GameEngine.Graphics.ModelDefinitions;
using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using KihonEngine.GameEngine.State;
using KihonEngine.Services;

namespace KihonEngine.Studio.Controls
{
    public static class GameEngineControllerExtentions
    {
        public static void AddModelAndNotify(this IGameEngineController controller, ModelBaseDefinition definition)
        {
            var modelBuilder = new ModelBuilderFromDefinition();
            var model = modelBuilder.Build(definition);

            Container.Get<IWorldEngine>().AddModel(model);
            Container.Get<IGameEngineState>().Editor.ActionSelect.SelectedModel = model;
            controller.NotifyIOs();
        }

        public static T GetDefinition<T>(this IGameEngineController controller, LayeredModel3D layeredModel) where T : ModelBaseDefinition
        {
            var definitionBuilder = new ModelDefinitionBuilder();
            return (T)definitionBuilder.CreateModelDefinition(layeredModel);
        }

        public static void ReplaceModelAndNotify(this IGameEngineController controller, LayeredModel3D modelToReplace, ModelBaseDefinition definition)
        {
            var modelBuilder = new ModelBuilderFromDefinition();
            var model = modelBuilder.Build(definition);

            Container.Get<IWorldEngine>().ReplaceModel(modelToReplace, model);
            Container.Get<IGameEngineState>().Editor.ActionSelect.SelectedModel = model;
            controller.NotifyIOs();
        }
    }
}
