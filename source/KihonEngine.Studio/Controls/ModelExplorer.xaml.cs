using KihonEngine.GameEngine;
using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using KihonEngine.GameEngine.State;
using KihonEngine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KihonEngine.Studio.Controls
{
    /// <summary>
    /// Interaction logic for ModelExplorer.xaml
    /// </summary>
    public partial class ModelExplorer : UserControl, ISynchronizedIO
    {
        private ILogService LogService
            => Container.Get<ILogService>();
        private IGameEngineState State
            => Container.Get<IGameEngineState>();
        private IGameEngineController GameEngineController
            => Container.Get<IGameEngineController>();

        private Timer timer;
        private int timerIntervalInMilliseconds = 1000;
        private bool _processing;

        private IGameEngineState _lastState;
        private bool _stateUpdated;

        public ModelExplorer()
        {
            InitializeComponent();

            timer = new Timer();
            timer.Interval = timerIntervalInMilliseconds;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        public void Synchronize(IGameEngineState state)
        {
            // Defered due to high refresh rate
            _lastState = state;
            _stateUpdated = true;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!_processing)
            {
                try
                {
                    _processing = true;

                    if (_lastState != null && _stateUpdated)
                    {
                        Dispatcher.Invoke(() => SynchronizeInternal(_lastState));
                        _stateUpdated = false;
                    }
                }
                catch (Exception ex)
                {
                    LogService.Log($"ERROR:{nameof(StateProperties)}:{ex.Message}");
                }
                finally
                {
                    _processing = false;
                }
            }
        }

        private void SynchronizeInternal(IGameEngineState state)
        {
            lvModels.IsEnabled = state.EngineMode == EngineMode.EditorMode;

            if (lvModels.IsEnabled)
            {
                var list = new List<ModelViewModel>();
                var level = state.Graphics.Level;
                var index = 0;
                foreach (var layeredModel in level)
                {
                    list.Add(CreateViewModel(index++, layeredModel));
                }

                var lvSelectedIndex = lvModels.SelectedIndex;
                if (state.Editor.ActionSelect.SelectedModel != null)
                {
                    lvSelectedIndex = level.IndexOf(state.Editor.ActionSelect.SelectedModel);
                }

                lvModels.ItemsSource = list;
                if (lvSelectedIndex > list.Count)
                {
                    lvSelectedIndex = list.Count - 1;
                }

                if (lvSelectedIndex != -1)
                {
                    lvModels.SelectedIndex = lvSelectedIndex;
                    lvModels.ScrollIntoView(list[lvSelectedIndex]);
                }
            }
            else
            {
                lvModels.ItemsSource = null;
            }
        }

        private void lvModels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvModels.SelectedItem != null && !_processing)
            {
                var viewModel = lvModels.SelectedItem as ModelViewModel;
                State.Editor.ActionSelect.SelectedModel = viewModel.Model;
                GameEngineController.NotifyIOs();
            }
        }

        private ModelViewModel CreateViewModel(int index, LayeredModel3D model)
        {
            var viewModel = new ModelViewModel();
            viewModel.Index = index.ToString();
            viewModel.Type = model.Type.ToString();
            viewModel.Color = model.GetColorFromMetadata().ToString();
            viewModel.Model = model;
            viewModel.Tags = string.Join(",", model.Tags.ToArray());

            if (model.Type != ModelType.Group)
            {
                var assemblyName = this.GetType().Assembly.GetName().Name;
                var sourceUri = $"pack://application:,,,/{assemblyName};component/Content/Images/Icons/icon-{model.Type.ToString().ToLower()}-transparent.png";
                viewModel.Icon = new BitmapImage(new Uri(sourceUri));
            }
            else
            {
                viewModel.Icon = null;
            }

            return viewModel;
        }

        public class ModelViewModel
        {
            public string Index { get; set; }
            public string Type { get; set; }
            public string Tags { get; set; }
            public string Color { get; set; }
            public ImageSource Icon { get; set; } 
            public LayeredModel3D Model { get; set; }
        }
    }
}
