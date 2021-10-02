using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media;

namespace KihonEngine.SampleMaps.Maze
{
    public class MazeNode
    {
        public MazeNode Predecessor { get; set; }

        public MazeNode North { get; set; }
        public MazeNode South { get; set; }
        public MazeNode East { get; set; }
        public MazeNode West { get; set; }

        public MazeNode[] Neighbors => new[] { North, South, East, West };

        public int X { get; set; }
        public int Z { get; set; }

        public int SizeX { get; set; }
        public int SizeY => 50;
        public int SizeZ { get; set; }
        public int WallSize => 1;
        public int FloorSize => 1;

        public MazeNode(int x, int z, int sizeX, int sizeZ)
        {
            X = x;
            Z = z;
            SizeX = sizeX;
            SizeZ = sizeZ;
        }

        public void DrawWalls(Graphics gr, System.Drawing.Pen pen, double xScale, double zScale)
        {
            for (int side = 0; side < 4; side++)
            {
                if ((Neighbors[side] == null) ||
                    ((Neighbors[side].Predecessor != this) &&
                     (Neighbors[side] != this.Predecessor)))
                {
                    DrawWall(gr, pen, (MazeNodeSide)side, xScale, zScale);
                }
            }
        }

        private void DrawWall(Graphics gr, System.Drawing.Pen pen, MazeNodeSide side, double xScale, double zScale)
        {
            var sizeX = (int)(SizeX * xScale);
            var sizeZ = (int)(SizeZ * zScale);

            switch (side)
            {
                case MazeNodeSide.North:
                    gr.DrawLine(pen,
                        X * sizeX, Z * sizeZ,
                        (X + 1) * sizeX, Z * sizeZ);
                    break;
                case MazeNodeSide.South:
                    gr.DrawLine(pen,
                        X * sizeX, (Z + 1) * sizeZ,
                        (X + 1) * sizeX, (Z + 1) * sizeZ);
                    break;
                case MazeNodeSide.East:
                    gr.DrawLine(pen,
                        (X + 1) * sizeX, Z * sizeZ,
                        (X + 1) * sizeX, (Z + 1) * sizeZ);
                    break;
                case MazeNodeSide.West:
                    gr.DrawLine(pen,
                        X * sizeX, Z * sizeZ,
                        X * sizeX, (Z + 1) * sizeZ);
                    break;
            }
        }

        public List<LayeredModel3D> GetModels()
        {
            var models = new List<LayeredModel3D>();
            var builder = new VolumeBuilder(Colors.White, models);

            builder.Create(X * SizeX, 0, Z * SizeZ, SizeX, FloorSize, SizeZ);

            for (int side = 0; side < 4; side++)
            {
                if ((Neighbors[side] == null) ||
                    ((Neighbors[side].Predecessor != this) &&
                     (Neighbors[side] != this.Predecessor)))
                {
                    BuildWall(builder, (MazeNodeSide)side);
                }
            }

            return models;
        }

        private void BuildWall(VolumeBuilder builder, MazeNodeSide side)
        {
            switch (side)
            {
                case MazeNodeSide.North:
                    builder.Create(X * SizeX, 0, Z * SizeZ, SizeX, SizeY, WallSize);
                    break;
                case MazeNodeSide.South:
                    builder.Create(X * SizeX, 0, (Z + 1) * SizeZ - WallSize, SizeX, SizeY, WallSize);
                    break;
                case MazeNodeSide.East:
                    builder.Create((X + 1) * SizeX - WallSize, 0, Z * SizeZ, WallSize, SizeY, SizeZ);
                    break;
                case MazeNodeSide.West:
                    builder.Create(X * SizeX, 0, Z * SizeZ, WallSize, SizeY, SizeZ);
                    break;
            }
        }
    }

}
