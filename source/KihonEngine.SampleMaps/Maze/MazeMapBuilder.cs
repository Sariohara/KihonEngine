using KihonEngine.GameEngine.Graphics.Maps;
using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace KihonEngine.SampleMaps.Maze
{
    public class MazeMapBuilder : IMapBuilder
    {
        public string MapName => $"Random Maze {SizeX} x {SizeZ}";

        public Point3D PlayerPosition => new Point3D(20, 16, 20);

        public Vector3D PlayerLookDirection => new Vector3D(1, 0, 1);

        public int SizeX { get; set; }
        public int SizeZ { get; set; }

        private MazeNode[,] _nodes;

        public MazeMapBuilder()
        {
            SizeX = 8;
            SizeZ = 12;
        }

        public MazeMapBuilder(int sizeX, int sizeZ, MazeNode[,] nodes)
        {
            SizeX = sizeX;
            SizeZ = sizeZ;
            _nodes = nodes;
        }

        public List<LayeredModel3D> CreateMap()
        {
            var level = new List<LayeredModel3D>();

            var lightBuilder = new LightBuilder(Colors.Transparent, level);
            var skyboxBuilder = new SkyboxBuilder(Colors.Transparent, level);

            // Lights
            lightBuilder.Create(new Vector3D(-3, -4, -5));
            lightBuilder.Create(new Vector3D(3, 4, 5));

            // Sky box
            skyboxBuilder.Color = Colors.AntiqueWhite;
            skyboxBuilder.Create(-5000, -5000, -5000, 10000, new Vector3D(-3, -4, -5));

            // Maze
            var mazeBuilder = new MazeBuilder();

            if (_nodes == null)
            {
                _nodes = mazeBuilder.MakeNodes(SizeX, SizeZ);
                mazeBuilder.FindSpanningTree(_nodes[0, 0]);
            }

            level.AddRange(mazeBuilder.CreateMazeModels(_nodes));

            return level;
        }
    }
}
