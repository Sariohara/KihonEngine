using KihonEngine.GameEngine.Graphics.Maps;
using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using KihonEngine.SampleMaps.Maze;
using KihonEngine.Studio.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KihonEngine.Studio.Controls
{
    /// <summary>
    /// Interaction logic for MazeEditorWindow.xaml
    /// </summary>
    public partial class MazeEditorWindow : Window
    {
        private const int MinMazeSize = 2;
        private const int MaxMazeSize = 15;

        private MazeNode[,] _nodes;

        public MazeEditorWindow()
        {
            InitializeComponent();

            XSize = 10;
            ZSize = 15;
        }

        public int XSize { get; set; }
        public int ZSize { get; set; }
        public IMapBuilder MapBuilder { get; set; }

        public void Synchronize()
        {
            cbXSize.Text = XSize.ToString();
            cbZSize.Text = ZSize.ToString();

            var bm = new Bitmap(
                (int)blockPreview.ActualWidth + 1,
                (int)blockPreview.ActualHeight + 1);

            var mazeBuilder = new MazeBuilder();
            
            using (Graphics gr = Graphics.FromImage(bm))
            {
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                var xScale = blockPreview.ActualWidth / ((_nodes.GetUpperBound(0) + 1) * mazeBuilder.NodeSize);
                var zScale = blockPreview.ActualHeight / ((_nodes.GetUpperBound(1) + 1) * mazeBuilder.NodeSize);
                mazeBuilder.DrawMaze(_nodes, gr, Pens.LightGray, xScale, zScale);
            }

            var imageSource = ImageHelper.ToBitmapImage(bm);
            blockPreview.Background = new ImageBrush(imageSource);
        }

        public void CreateMaze()
        {
            if (XSize < MinMazeSize)
            {
                XSize = MinMazeSize;
            }

            if (ZSize < MinMazeSize)
            {
                ZSize = MinMazeSize;
            }

            if (XSize > MaxMazeSize)
            {
                XSize = MaxMazeSize;
            }

            if (ZSize > MaxMazeSize)
            {
                ZSize = MaxMazeSize;
            }

            var mazeBuilder = new MazeBuilder();
            _nodes = mazeBuilder.MakeNodes(XSize, ZSize);
            mazeBuilder.FindSpanningTree(_nodes[0, 0]);
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        private void btLoadMap_Click(object sender, RoutedEventArgs e)
        {
            MapBuilder = new MazeMapBuilder(XSize, ZSize, _nodes);
            DialogResult = true;
            Close();
        }

        private void btGenerate_Click(object sender, RoutedEventArgs e)
        {
            InputHelper.TryUpdate(cbXSize.Text, x => XSize = x);
            InputHelper.TryUpdate(cbZSize.Text, z => ZSize = z);

            CreateMaze();
            Synchronize();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var availableSizes = new List<int>();
            for (int i = MinMazeSize; i <= MaxMazeSize; i++)
                availableSizes.Add(i);
            cbXSize.ItemsSource = availableSizes;
            cbZSize.ItemsSource = availableSizes;

            CreateMaze();
            Synchronize();
        }
    }
}
