using KihonEngine.GameEngine.Graphics;
using KihonEngine.GameEngine.Graphics.Maps;
using KihonEngine.GameEngine.State;
using KihonEngine.Services;
using System.Collections.Generic;
using System.Linq;

namespace KihonEngine
{
    public static class StandardStartups
    {
        /// <summary>
        /// Build a Full Screen mode game with a specific map file
        /// </summary>
        /// <param name="mapFile">file of the map to register</param>
        public static void BuildStandaloneFullScreenGame(string mapFile)
        {
            BuildStandaloneFullScreenGame(new[] { new MapBuilderFromFile(mapFile) });
        }

        /// <summary>
        /// Build a Full Screen mode game with a specific set of map files
        /// </summary>
        /// <param name="mapFiles">files of the maps to register</param>
        public static void BuildStandaloneFullScreenGame(string[] mapFiles)
        {
            BuildStandaloneFullScreenGame(mapFiles.Select(x => new MapBuilderFromFile(x)));
        }

        /// <summary>
        /// Build a Full Screen mode game with a specific IMapBuilder class to use for map generation
        /// </summary>
        /// <typeparam name="TMapBuilder">IMapBuilder class to use for map generation</typeparam>
        public static void BuildStandaloneFullScreenGame<TMapBuilder>() where TMapBuilder : class, IMapBuilder, new()
        {
            BuildStandaloneFullScreenGame(new[] { new TMapBuilder() });
        }

        /// <summary>
        /// Build a Full Screen mode game with a specific IMapBuilder class to use for map generation
        /// </summary>
        /// <param name="mapBuilder">IMapBuilder class to use for map generation</param>
        public static void BuildStandaloneFullScreenGame(IMapBuilder mapBuilder)
        {
            BuildStandaloneFullScreenGame(new[] { mapBuilder });
        }

        /// <summary>
        /// Build a Full Screen mode game with a set of specific IMapBuilder classes to use for map generation
        /// </summary>
        /// <param name="mapBuilders">Set of IMapBuilder classes to use for map generation</param>
        public static void BuildStandaloneFullScreenGame(IEnumerable<IMapBuilder> mapBuilders)
        {
            var state = Container.Get<IGameEngineState>();
            var worldEngine = Container.Get<IWorldEngine>();

            state.Game.IsStandaloneFullScreenGame = true;

            foreach (var mapBuilder in mapBuilders)
            {
                worldEngine.RegisterMap(mapBuilder);
            }

            Engine.Controller.RegisterDefaultGraphicOutput();
            Engine.Controller.SwitchToNormalScreen();

            // Load map
            Engine.Controller.LoadMap(worldEngine.RegisteredMaps[0]);
        }
    }
}
