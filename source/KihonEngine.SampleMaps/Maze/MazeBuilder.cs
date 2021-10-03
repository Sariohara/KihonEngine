using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace KihonEngine.SampleMaps.Maze
{
    public class MazeBuilder
    {
        public int NodeSize => 40;

        public MazeNode[,] MakeNodes(int sizeX, int sizeZ)
        {
            // Make the nodes.
            MazeNode[,] nodes = new MazeNode[sizeX, sizeZ];
            for (int x = 0; x < sizeX; x++)
            {
                for (int z = 0; z < sizeZ; z++)
                {
                    nodes[x, z] = new MazeNode(x, z, NodeSize, NodeSize);
                }
            }

            // Initialize the nodes' neighbors.
            for (int x = 0; x < sizeX; x++)
            {
                for (int z = 0; z < sizeZ; z++)
                {
                    if (z > 0)
                        nodes[x, z].North = nodes[x, z - 1];
                    if (z < sizeZ - 1)
                        nodes[x, z].South = nodes[x, z + 1];
                    if (x > 0)
                        nodes[x, z].West = nodes[x - 1, z];
                    if (x < sizeX - 1)
                        nodes[x, z].East = nodes[x + 1, z];
                }
            }

            // Return the nodes.
            return nodes;
        }

        public void FindSpanningTree(MazeNode root)
        {
            Random rand = new Random();

            // Set the root node's predecessor so we know it's in the tree.
            root.Predecessor = root;

            // Make a list of candidate links.
            List<MazeLink> links = new List<MazeLink>();

            // Add the root's links to the links list.
            foreach (MazeNode neighbor in root.Neighbors)
            {
                if (neighbor != null)
                {
                    links.Add(new MazeLink(root, neighbor));
                }
            }

            // Add the other nodes to the tree.
            while (links.Count > 0)
            {
                // Pick a random link.
                int link_num = rand.Next(0, links.Count);
                MazeLink link = links[link_num];
                links.RemoveAt(link_num);

                // Add this link to the tree.
                MazeNode to_node = link.To;
                link.To.Predecessor = link.From;

                // Remove any links from the list that point
                // to nodes that are already in the tree.
                // (That will be the newly added node.)
                for (int i = links.Count - 1; i >= 0; i--)
                {
                    if (links[i].To.Predecessor != null)
                        links.RemoveAt(i);
                }

                // Add to_node's links to the links list.
                foreach (MazeNode neighbor in to_node.Neighbors)
                {
                    if ((neighbor != null) && (neighbor.Predecessor == null))
                        links.Add(new MazeLink(to_node, neighbor));
                }
            }
        }

        public List<LayeredModel3D> CreateMazeModels(MazeNode[,] nodes)
        {
            int sizeX = nodes.GetUpperBound(0) + 1;
            int sizeZ = nodes.GetUpperBound(1) + 1;
            var models = new List<LayeredModel3D>();
            for (int x = 0; x < sizeX; x++)
            {
                for (int z = 0; z < sizeZ; z++)
                {
                    models.AddRange(nodes[x, z].GetModels());
                }
            }

            return models;
        }

        public void DrawMaze(MazeNode[,] nodes, Graphics gr, Pen pen, double xScale, double zScale)
        {
            int sizeX = nodes.GetUpperBound(0) + 1;
            int sizeZ = nodes.GetUpperBound(1) + 1;
            for (int x = 0; x < sizeX; x++)
            {
                for (int z = 0; z < sizeZ; z++)
                {
                    nodes[x, z].DrawWalls(gr, pen, xScale, zScale);
                }
            }
        }
    }
}
