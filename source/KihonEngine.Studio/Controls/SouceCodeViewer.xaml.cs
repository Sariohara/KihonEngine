using System;
using System.Timers;
using System.Windows.Controls;
using KihonEngine.GameEngine;
using KihonEngine.GameEngine.Graphics.ModelDefinitions;
using KihonEngine.GameEngine.State;
using KihonEngine.Services;
using Newtonsoft.Json;

namespace KihonEngine.Studio.Controls
{
    /// <summary>
    /// Interaction logic for SouceCodeViewer.xaml
    /// </summary>
    public partial class SouceCodeViewer : UserControl, ISynchronizedIO
    {
        private ILogService LogService
            => Container.Get<ILogService>();

        private Timer timer;
        private int timerIntervalInMilliseconds = 2000;
        private bool processing;

        private IGameEngineState _lastState;
        private bool _stateUpdated;

        public SouceCodeViewer()
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
            if (state.EngineMode == EngineMode.EditorMode)
            {
                _lastState = state;
                _stateUpdated = true;
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!processing)
            {
                try
                {
                    processing = true;

                    if (_lastState != null && _stateUpdated)
                    {
                        Dispatcher.Invoke(() => SynchronizeInternal(_lastState));
                        _stateUpdated = false;
                    }
                }
                catch (Exception ex)
                {
                    LogService.Log($"ERROR:{nameof(SouceCodeViewer)}:{ex.Message}");
                }
                finally
                {
                    processing = false;
                }
            }
        }

        private void SynchronizeInternal(IGameEngineState state)
        {
            var builder = new ModelDefinitionBuilder();
            var mapDefinition = builder.CreateMapDefinition(
                state.Graphics.LevelName,
                state.Graphics.RespawnPosition,
                state.Graphics.RespawnLookDirection,
                state.Graphics.Level);

            string json = JsonConvert.SerializeObject(mapDefinition, Formatting.Indented);

            textBox.Text = json;
        }
    }
}
