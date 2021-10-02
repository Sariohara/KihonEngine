
namespace KihonEngine.SampleMaps.Maze
{
    internal class MazeLink
    {
        public MazeNode From { get; set; }
        public MazeNode To { get; set; }

        public MazeLink(MazeNode from, MazeNode to)
        {
            From = from;
            To = to;
        }
    }
}
