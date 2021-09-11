using System.Linq;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using KihonEngine.GameEngine.Graphics.ModelDefinitions;
using KihonEngine.GameEngine.Graphics.ModelsBuilders;

namespace KihonEngine.GameEngine.Graphics.Maps
{
    public class MapBuilderFromDefinition : IMapBuilder
    {
        public string MapName => _mapDefinition.Name;

        public Point3D PlayerPosition => _mapDefinition.PlayerPosition;

        public Vector3D PlayerLookDirection => _mapDefinition.PlayerLookDirection;

        private MapDefinition _mapDefinition;

        public MapBuilderFromDefinition(MapDefinition mapDefinition)
        {
            _mapDefinition = mapDefinition;
        }

        public List<LayeredModel3D> CreateMap()
        {
            var level = new List<LayeredModel3D>();

            var definitions = new List<ModelBaseDefinition>();
            definitions.AddRange(_mapDefinition.Ceilings);
            definitions.AddRange(_mapDefinition.Floors);
            definitions.AddRange(_mapDefinition.Lights);
            definitions.AddRange(_mapDefinition.Skyboxes);
            definitions.AddRange(_mapDefinition.Volumes);
            definitions.AddRange(_mapDefinition.Walls);

            definitions = definitions.OrderBy(x => x.Index).ToList();

            var builder = new ModelBuilderFromDefinition();
            foreach (var definition in definitions)
            {
                level.Add(builder.Build(definition));
            }

            return level;
        }
    }
}
