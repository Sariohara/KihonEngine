using KihonEngine.GameEngine;
using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using KihonEngine.GameEngine.State;
using KihonEngine.Services;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace KihonEngine.Studio.Controls
{
    /// <summary>
    /// Interaction logic for ModelExplorer.xaml
    /// </summary>
    public partial class ModelExplorer : UserControl, ISynchronizedIO
    {
        private IGameEngineState State
            => Container.Get<IGameEngineState>();
        private IGameEngineController GameEngineController
            => Container.Get<IGameEngineController>();

        public ModelExplorer()
        {
            InitializeComponent();
        }

        private bool _synchronizing;
        public void Synchronize(IGameEngineState state)
        {
            _synchronizing = true;

            lvModels.IsEnabled = state.EngineMode == EngineMode.EditorMode;

            if (lvModels.IsEnabled)
            {
                var list = new List<ModelViewModel>();
                var level = state.Graphics.Level.ToArray();
                var index = 0;
                foreach (var layeredModel in level)
                {
                    var viewModel = new ModelViewModel();
                    viewModel.Index = (index++).ToString();
                    viewModel.Type = layeredModel.Type.ToString();
                    viewModel.Color = layeredModel.GetColor();
                    viewModel.Model = layeredModel;

                    list.Add(viewModel);
                }

                lvModels.ItemsSource = list;
            }
            else
            {
                lvModels.ItemsSource = null;
            }

            _synchronizing = false;
        }

        public class ModelViewModel
        {
            public string Index { get; set; }
            public string Type { get; set; }
            public Color Color { get; set; }
            public LayeredModel3D Model { get; set; }
        }

        private void lvModels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvModels.SelectedItem != null && !_synchronizing)
            {
                var viewModel = lvModels.SelectedItem as ModelViewModel;
                State.Editor.ActionSelect.SelectedModel = viewModel.Model;
                GameEngineController.NotifyIOs();
            }
        }
    }
}
