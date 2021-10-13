using KihonEngine.GameEngine;
using KihonEngine.GameEngine.Configuration;
using KihonEngine.GameEngine.GameLogics.Editor;
using KihonEngine.GameEngine.Graphics;
using KihonEngine.GameEngine.Graphics.Content;
using KihonEngine.GameEngine.Graphics.Maps;
using KihonEngine.GameEngine.InputControls;
using KihonEngine.GameEngine.State;
using KihonEngine.Services;

namespace KihonEngine
{
    internal static class EngineStartup
    {
        public static void ConfigureServices(IServiceLocator locator)
        {
            // Generic services
            locator.Register<ILogService>(new LogService());

            // Engine State
            locator.Register<IGameEngineState>(new GameEngineState());

            // Engine services
            locator.Register<ICursorController>(new CursorController());
            locator.Register<IConfigurationService>(new ConfigurationService());
            locator.Register<IMapStorageService>(new MapStorageService());
            locator.Register<IMapBuilderFactory>(new MapBuilderFactory());

            var contentService = new ContentService();
            contentService.RegisterSource(new EmbeddedContentSource());
            locator.Register<IContentService>(contentService);

            locator.Register<IWorldEngine>(new WorldEngine());
            locator.Register<ICameraController>(new CameraController());

            // Editor Services
            locator.Register<INewModelManager>(new NewModelManager());

            // Engine main controller
            locator.Register<IGameEngineController>(new GameEngineController());
        }
    }
}
