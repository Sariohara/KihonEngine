using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;
using System.Collections.Concurrent;
using KihonEngine.Services;
using KihonEngine.GameEngine.State;
using KihonEngine.GameEngine.Graphics;
using KihonEngine.GameEngine.Configuration;
using KihonEngine.GameEngine.InputControls;
using System.Threading;
using System.Threading.Tasks;

namespace KihonEngine.GameEngine.GameLogics
{
    public abstract class GameLogicBase : IGameLogic
    {
        public abstract string LogicName { get; }
        public bool IsRunning => _gameLogicThread != null && _gameLogicThread.IsAlive;

        protected ILogService LogService { get; set; }
        protected IGameEngineState State { get; set; }
        protected ICameraController CameraController { get; set; }
        protected IWorldEngine WorldEngine { get; set; }
        protected IConfigurationService Configuration { get; set; }

        public event EventHandler StateChanged;

        protected HashSet<Key> _keyboardPressedKeys = new HashSet<Key>();
        protected ConcurrentQueue<MouseEvent> _mouseEvents = new ConcurrentQueue<MouseEvent>();

        private Thread _gameLogicThread;
        private CancellationTokenSource _cts;

        public GameLogicBase()
        {
            LogService = Container.Get<ILogService>();
            Configuration = Container.Get<IConfigurationService>();
            State = Container.Get<IGameEngineState>();
            WorldEngine = Container.Get<IWorldEngine>();
            CameraController = Container.Get<ICameraController>();
        }

        public void Start()
        {
            _cts = new CancellationTokenSource();
            LogService.Log($"Start {LogicName} thread");
            _gameLogicThread = new Thread(() => MainLoop(_cts.Token));
            _gameLogicThread.Start();
        }

        public void Stop()
        {
            if (_gameLogicThread != null)
            {
                _cts.Cancel();
                LogService.Log($"Signal sent for stop {LogicName} thread");

                //_gameLogicThread.Join(3000); // Deadlock removed
                _gameLogicThread = null;
            }
        }

        public void PublishMouseClick(System.Windows.Point position, MouseButtonState leftButton, MouseButtonState rightButton, int clickCount)
        {
            if (_gameLogicThread != null && _gameLogicThread.IsAlive)
            {
                _mouseEvents.Enqueue(new MouseEvent { Type = MouseEventType.Click, Position = position, LeftButton = leftButton, RightButton = rightButton });
            }
        }

        public void PublishMouseMove(System.Windows.Point position, MouseButtonState leftButton, MouseButtonState rightButton, double dx, double dy)
        {
            if (_gameLogicThread != null && _gameLogicThread.IsAlive)
            {
                _mouseEvents.Enqueue(new MouseEvent { Type = MouseEventType.Move, Position = position, LeftButton = leftButton, RightButton = rightButton, DX = dx, DY = dy });
            }
        }

        public void PublishMouseWheel(System.Windows.Point position, MouseButtonState leftButton, MouseButtonState rightButton, double delta)
        {
            if (_gameLogicThread != null && _gameLogicThread.IsAlive)
            {
                _mouseEvents.Enqueue(new MouseEvent { Type = MouseEventType.Wheel, Position = position, LeftButton = leftButton, RightButton = rightButton, Delta = delta });
            }
        }

        public void RegisterKeyboardPressedKey(Key key)
        {
            if (_gameLogicThread != null && _gameLogicThread.IsAlive)
            {
                if (!_keyboardPressedKeys.Contains(key))
                {
                    _keyboardPressedKeys.Add(key);
                    State.KeyPressed = _keyboardPressedKeys.ToArray();
                }
            }
        }

        public void RemoveKeyboardPressedKey(Key key)
        {
            if (_gameLogicThread != null && _gameLogicThread.IsAlive)
            {
                if (_keyboardPressedKeys.Contains(key))
                {
                    _keyboardPressedKeys.Remove(key);
                    State.KeyPressed = _keyboardPressedKeys.ToArray();
                }
            }
        }

        protected void Wait(int millisecondsDelay, CancellationToken stoppingToken)
        {
            try
            {
                Task.Delay(10, stoppingToken).Wait();
            }
            catch (Exception)
            {

            }
        }

        protected void OnStateChanged()
        {
            if (StateChanged != null)
            {
                State.Graphics.Viewport.Dispatcher.Invoke(() => StateChanged(this, null));
            }
        }

        protected abstract void MainLoop(CancellationToken stoppingToken);
    }
}
