﻿using KihonEngine.GameEngine;
using KihonEngine.GameEngine.State;
using KihonEngine.GameEngine.State.FpsGame;
using KihonEngine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace KihonEngine.Studio.Controls
{
    /// <summary>
    /// Interaction logic for StateProperties.xaml
    /// </summary>
    public partial class StateProperties : UserControl, ISynchronizedIO
    {
        private ILogService LogService
            => Container.Get<ILogService>();

        private Timer timer;
        private int timerIntervalInMilliseconds = 500;
        private bool processing;

        private IGameEngineState _lastState;
        private bool _stateUpdated;

        public StateProperties()
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
                catch(Exception ex)
                {
                    LogService.Log($"ERROR:{nameof(StateProperties)}:{ex.Message}");
                }
                finally
                {
                    processing = false;
                }
            }
        }

        private void SynchronizeInternal(IGameEngineState state)
        {
            var properties = new List<StateItemPropertyViewModel>();
            AddProperty(properties, "Global", string.Empty);
            AddProperty(properties, "  EngineMode", state.EngineMode.ToString());
            AddProperty(properties, "  CurrentLogicName", state.CurrentLogicName.ToString());
            AddProperty(properties, "  CanHandleMouseMoves", state.CanHandleMouseMoves.ToString());
            AddProperty(properties, "  KeyPressed", state.KeyPressed != null ? string.Join(",", state.KeyPressed.Select(x => x.ToString())) : string.Empty);
            AddProperty(properties, "Game", string.Empty);
            AddProperty(properties, "  Game.LifeState.Life", state.Game.Get<LifeState>().Life.ToString());
            AddProperty(properties, "  Game.JumpState.IsJumping", state.Game.Get<JumpState>().IsJumping.ToString());
            AddProperty(properties, "  Game.JumpState.YSpeed", state.Game.Get<JumpState>().YSpeed.ToString());
            AddProperty(properties, "  Game.JumpState.Gravity", state.Game.Get<JumpState>().Gravity.ToString());
            AddProperty(properties, "Editor", string.Empty);
            AddProperty(properties, "  Editor.CurrentColor", state.Editor.CurrentColor.ToString());
            AddProperty(properties, "  Editor.RotationStep", state.Editor.RotationStep.ToString());
            AddProperty(properties, "  Editor.TranslationStep", state.Editor.TranslationStep.ToString());
            AddProperty(properties, "  Editor.ActionMove.Mode", state.Editor.ActionMove.Mode.ToString());
            AddProperty(properties, "  Editor.ActionMove.InvisibleMeterModel", state.Editor.ActionMove.InvisibleMeterModel != null ? "<object>" : "<null>");
            AddProperty(properties, "  Editor.ActionSelect.InitialModelPosition", state.Editor.ActionSelect.InitialModelPosition.ToString());
            AddProperty(properties, "  Editor.ActionSelect.SelectionBoxModel", state.Editor.ActionSelect.SelectionBoxModel != null? "<object>" : "<null>");

            var SelectedIndex = "<null>";
            if (state.Editor.ActionSelect.SelectedModel != null)
            {
                SelectedIndex = state.Graphics.Level.IndexOf(state.Editor.ActionSelect.SelectedModel).ToString();
            }
            
            AddProperty(properties, "  Editor.ActionSelect.SelectedIndex", SelectedIndex);
            AddProperty(properties, "Graphics", string.Empty);
            AddProperty(properties, "  Graphics.Level", state.Graphics.Level.Count.ToString() + " model(s)");
            AddProperty(properties, "  Graphics.LevelName", state.Graphics.LevelName);
            AddProperty(properties, "  Graphics.PlayerCamera.Camera.Position", state.Graphics.PlayerCamera.Camera.Position.ToString());
            AddProperty(properties, "  Graphics.PlayerCamera.Camera.TransformedPosition", state.Graphics.PlayerCamera.GetTransformedPosition().ToString());
            AddProperty(properties, "  Graphics.PlayerCamera.Camera.LookDirection", state.Graphics.PlayerCamera.Camera.LookDirection.ToString());

            var lvSelectedIndex = lvStateProperties.SelectedIndex;
            lvStateProperties.ItemsSource = properties;
            lvStateProperties.SelectedIndex = lvSelectedIndex;
        }

        private void AddProperty(List<StateItemPropertyViewModel> properties, string name, string value)
        {
            properties.Add(new StateItemPropertyViewModel { Name = name, Value = value });
        }

        public class StateItemPropertyViewModel
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }
    }
}
