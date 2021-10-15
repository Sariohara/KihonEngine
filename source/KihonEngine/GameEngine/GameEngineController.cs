using System;
using System.Collections.Generic;
using System.Windows.Input;
using KihonEngine.Services;
using KihonEngine.GameEngine.State;
using KihonEngine.GameEngine.Graphics;
using KihonEngine.GameEngine.Configuration;
using KihonEngine.GameEngine.Graphics.ModelDefinitions;
using KihonEngine.GameEngine.InputControls;
using System.Windows.Controls;
using KihonEngine.GameEngine.Graphics.Output;
using KihonEngine.GameEngine.Graphics.Maps.Predefined;
using KihonEngine.GameEngine.GameLogics;
using KihonEngine.GameEngine.Graphics.Maps;

namespace KihonEngine.GameEngine
{
    public class GameEngineController : IGameEngineController
    {
        private ILogService LogService { get; set; }
        private IConfigurationService Configuration { get; set; }
        private IMapStorageService MapStorageService { get; set; }
        private ICursorController CursorController { get; set; }
        private IGameEngineState State { get; set; }
        private ICameraController CameraController { get; set; }
        private IWorldEngine WorldEngine { get; set; }

        private List<ISynchronizedIO> _externalIOs;
        private Func<IGraphicOutput> _normalScreenGraphicProvider;
        private Func<IGraphicOutput> _fullScreenGraphicProvider;
        private IGraphicOutput _currentGraphic;
        private GraphicOutputType _currentGraphicType;

        private IGameLogic _gameLogic;

        private bool needToCenterMouseBeforeNextMove;
        private bool skipNextMouseMoveEvent;
        private System.Windows.Point mousePosition;

        public GameEngineController()
        {
            _externalIOs = new List<ISynchronizedIO>();
        
            LogService = Container.Get<ILogService>();
            Configuration = Container.Get<IConfigurationService>();
            MapStorageService = Container.Get<IMapStorageService>();
            CursorController = Container.Get<ICursorController>();
            State = Container.Get<IGameEngineState>();
            WorldEngine = Container.Get<IWorldEngine>();
            CameraController = Container.Get<ICameraController>();

            LoadEmptyMap();

            State.Graphics.Viewport.SizeChanged += Viewport_SizeChanged;
        }

        public void LoadEmptyMap()
        {
            LoadMap(PredefinedMapNames.Empty);
        }

        public void LoadMap(IMapBuilder mapBuilder)
        {
            WorldEngine.LoadMap(mapBuilder);

            State.Editor.ActionSelect.SelectedModel = null;
            NotifyIOs();
        }

        public void LoadMap(string mapName)
        {
            WorldEngine.LoadMap(mapName);

            State.Editor.ActionSelect.SelectedModel = null;
            NotifyIOs();
        }

        public void LoadMapFromFile(string filepath)
        {
            WorldEngine.LoadMap("file:" + filepath);
        }

        public void SaveMapToFile(string filepath)
        {
            var builder = new ModelDefinitionBuilder();
            var mapDefinition = builder.CreateMapDefinition(
                State.Graphics.LevelName,
                State.Graphics.RespawnPosition,
                State.Graphics.RespawnLookDirection,
                State.Graphics.Level);

            MapStorageService.Save(filepath, mapDefinition);
        }

        public void RegisterIO(ISynchronizedIO input)
        {
            _externalIOs.Add(input);
        }

        public void RemoveIO(ISynchronizedIO input)
        {
            _externalIOs.Remove(input);
        }

        public UserControl RegisterDefaultGraphicOutput()
        {
            var viewportControl = new WorldVizualizer();
            
            RegisterGraphicOutput(
                normalScreenProvider: () => viewportControl,
                fullscreenProvider: () => new WorldVisualizeFullScreenWindow());

            return viewportControl;
        }

        public UserControl GetDefaultGraphicOutput()
        {
            return (UserControl)_normalScreenGraphicProvider();
        }

        public void RegisterGraphicOutput(Func<IGraphicOutput> normalScreenProvider, Func<IGraphicOutput> fullscreenProvider = null)
        {
            if (normalScreenProvider == null || fullscreenProvider == null)
            {
                throw new ArgumentNullException();
            }

            _normalScreenGraphicProvider = normalScreenProvider;
            _fullScreenGraphicProvider = fullscreenProvider;
        }

        public void SwitchToNormalScreen()
        {
            if (_currentGraphicType == GraphicOutputType.NormalScreen)
            {
                return;
            }

            if (_currentGraphic != null)
            {
                _currentGraphic.DetachViewport(State.Graphics.Viewport);
            }
    
            _currentGraphic = _normalScreenGraphicProvider();
            _currentGraphicType = GraphicOutputType.NormalScreen;
            _currentGraphic.AttachViewport(State.Graphics.Viewport);

            NotifyIOs();

            if (State.EngineMode == EngineMode.PlayMode)
            {
                needToCenterMouseBeforeNextMove = true;
            }
        }

        public void SwitchToFullScreen()
        {
            if (_currentGraphicType == GraphicOutputType.FullScreen)
            {
                return;
            }

            if (_currentGraphic != null)
            {
                _currentGraphic.DetachViewport(State.Graphics.Viewport);
            }

            _currentGraphic = _fullScreenGraphicProvider();
            _currentGraphicType = GraphicOutputType.FullScreen;
            _currentGraphic.AttachViewport(State.Graphics.Viewport);

            State.Graphics.Viewport.Focus();
            NotifyIOs();

            if (State.EngineMode == EngineMode.PlayMode)
            {
                needToCenterMouseBeforeNextMove = true;
            }
        }

        public void SwitchScreen()
        {
            if (_currentGraphic == null || _currentGraphicType == GraphicOutputType.None)
            {
                return;
            }

            if (_currentGraphicType == GraphicOutputType.NormalScreen)
            {
                SwitchToFullScreen();
            }
            else if (_currentGraphicType == GraphicOutputType.FullScreen)
            {
                SwitchToNormalScreen();
            }
        }

        public void StopGameLogic()
        {
            // Stop game logic
            _gameLogic?.Stop();

            // Mouse
            State.Graphics.Viewport.Cursor = Cursors.Arrow;

            // Set state
            State.EngineMode = EngineMode.Off;
            State.CurrentLogicName = string.Empty;

            // Respawn
            CameraController.Respawn();

            State.Graphics.Viewport.Focus();
            NotifyIOs();
        }

        public void Play<TMapBuilder>() where TMapBuilder : class, IMapBuilder, new()
        {
            Play(new TMapBuilder());
        }

        public void Play(IMapBuilder mapBuilder)
        {
            LoadMap(mapBuilder);
            Play();
        }

        public void Play()
        {
            if (State.Game.IsStandaloneFullScreenGame)
            {
                SwitchToFullScreen();
            }

            SwitchToPlayMode();
        }

        public void SwitchToPlayMode()
        {           
            SwitchToGameLogic<GameLogics.Fps.FpsGameLogic>(EngineMode.PlayMode, () => 
            {
                State.Graphics.Viewport.Cursor = Cursors.None;
                State.Graphics.Viewport.CaptureMouse();
            });
        }

        public void SwitchToEditorMode()
        {
            SwitchToGameLogic<GameLogics.Editor.EditorLogic>(EngineMode.EditorMode, () => { });
        }

        private void SwitchToGameLogic<TGameLogic>(EngineMode mode, Action configure) where TGameLogic : class, IGameLogic, new()
        {
            // Stop game logic
            _gameLogic?.Stop();

            // Mouse
            State.Graphics.Viewport.Cursor = Cursors.Arrow;
            State.Graphics.Viewport.ReleaseMouseCapture();
            State.CanHandleMouseMoves = true;

            // Set state
            configure();

            State.EngineMode = mode;
            State.Editor.ActionSelect.SelectedModel = null;

            // Respawn
            CameraController.Respawn();

            State.Graphics.Viewport.Focus();
            NotifyIOs();

            // Start Editor
            _gameLogic = new TGameLogic();

            _gameLogic.StateChanged += _gameLogic_StateChanged;

            State.CurrentLogicName = _gameLogic.LogicName;
            _gameLogic.Start();
        }

        public void DispatchKeyboardEvent(KeyEventArgs e)
        {
            if (_gameLogic == null || !_gameLogic.IsRunning)
            {
                return;
            }

            LogService.Log($"{e.RoutedEvent} : {e.Key}:{e.SystemKey}");

            var keyboardSettings = Configuration.GetKeyboardSettings();

            // Controls
            if (e.Key == keyboardSettings.CancelOperation && e.RoutedEvent.Name == "KeyDown" && State.EngineMode == EngineMode.PlayMode)
            {
                if (State.Game.IsStandaloneFullScreenGame)
                {
                    SwitchToNormalScreen();
                    StopGameLogic();
                }
                else
                {
                    SwitchToNormalScreen();
                    SwitchToEditorMode();
                }
            }
            else if (e.Key == keyboardSettings.FullScreenMode && e.RoutedEvent.Name == "KeyDown")
            {
                if (!(State.EngineMode == EngineMode.PlayMode && State.Game.IsStandaloneFullScreenGame))
                {
                    SwitchScreen();
                }
            }
            else if (e.Key != Key.System) //if (keyboardSettings.MoveKeys.Contains(e.Key) || keyboardSettings.LookKeys.Contains(e.Key))
            {
                if (e.RoutedEvent.Name == "KeyDown")
                {
                    _gameLogic.RegisterKeyboardPressedKey(e.Key);
                }

                if (e.RoutedEvent.Name == "KeyUp")
                {
                    _gameLogic.RemoveKeyboardPressedKey(e.Key);
                }
            }

            // handle system keys (like moving focus to menu bar when press F10)
            else if (e.Key == Key.System && e.RoutedEvent.Name == "KeyDown"
                && (_currentGraphicType == GraphicOutputType.FullScreen
                    || State.EngineMode == EngineMode.PlayMode))
            {
                e.Handled = true;
            }
        }

        public void DispatchMouseWheelEvent(MouseWheelEventArgs e)
        {
            if (_gameLogic == null || !_gameLogic.IsRunning)
            {
                return;
            }

            LogService.Log($"{e.RoutedEvent.Name} Delta:{e.Delta} LeftButton:{e.LeftButton} RightButton:{e.RightButton}");

            mousePosition = Mouse.GetPosition(State.Graphics.Viewport);

            _gameLogic.PublishMouseWheel(mousePosition, e.LeftButton, e.RightButton, e.Delta);
        }

        public void DispatchMouseButtonEvent(MouseButtonEventArgs e)
        {
            if (_gameLogic == null || !_gameLogic.IsRunning)
            {
                return;
            }

            LogService.Log($"{e.RoutedEvent.Name} ButtonState:{e.ButtonState} LeftButton:{e.LeftButton} RightButton:{e.RightButton}");
            
            mousePosition = Mouse.GetPosition(State.Graphics.Viewport);

            _gameLogic.PublishMouseClick(mousePosition, e.LeftButton, e.RightButton, e.ClickCount);
        }

        public void DispatchMouseEvent(MouseEventArgs e)
        {
            if (_gameLogic == null || !_gameLogic.IsRunning)
            {
                return;
            }

            var previousMousePosition = mousePosition;
            mousePosition = Mouse.GetPosition(State.Graphics.Viewport);
            LogService.Log($"{e.RoutedEvent.Name} : LeftButton:{e.LeftButton} RightButton:{e.RightButton}");
            LogService.Log($"{e.RoutedEvent.Name} : previous position x:{mousePosition.X}, y:{mousePosition.Y}");
            LogService.Log($"{e.RoutedEvent.Name} : current position x:{mousePosition.X}, y:{mousePosition.Y}");
            LogService.Log($"GameMode:{State.EngineMode};CanHandleMouseMoves:{State.CanHandleMouseMoves};needToCenterMouse:{needToCenterMouseBeforeNextMove}");

            if (needToCenterMouseBeforeNextMove)
            {
                LogService.Log($"Center mouse");
                needToCenterMouseBeforeNextMove = false;
                mousePosition = CursorController.CenterCursorPosition(State.Graphics.Viewport);
                return;
            }

            if (e.RoutedEvent.Name == "MouseEnter")
            {
                State.Graphics.Viewport.Focus();
                skipNextMouseMoveEvent = true;
            }
            else if (e.RoutedEvent.Name == "MouseMove" && State.CanHandleMouseMoves)
            {
                if (skipNextMouseMoveEvent)
                {
                    skipNextMouseMoveEvent = false;
                    LogService.Log($"{e.RoutedEvent.Name} : Skipped");
                    return;
                }

                var dx = mousePosition.X - previousMousePosition.X;
                var dy = mousePosition.Y - previousMousePosition.Y;

                LogService.Log($"{e.RoutedEvent.Name} : previous position x:{previousMousePosition.X}, y:{previousMousePosition.Y}");
                LogService.Log($"{e.RoutedEvent.Name} : new position      x:{mousePosition.X}, y:{mousePosition.Y}");
                LogService.Log($"{e.RoutedEvent.Name} : mouse move        DX:{dx}, dy:{dy}");

                _gameLogic.PublishMouseMove(mousePosition, e.LeftButton, e.RightButton, dx, dy);

                if (State.EngineMode == EngineMode.PlayMode)
                {
                    mousePosition = CursorController.CenterCursorPosition(State.Graphics.Viewport);
                }
            }
        }

        public void NotifyIOs()
        {
            if (_currentGraphic != null)
            {
                _currentGraphic.Synchronize(State);
            }

            foreach (var input in _externalIOs)
            {
                input.Synchronize(State);
            }
        }

        private void _gameLogic_StateChanged(object sender, EventArgs e)
        {
            NotifyIOs();
        }

        private void Viewport_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            LogService.Log($"size changed");
            if (State.EngineMode == EngineMode.PlayMode)
            {
                mousePosition = CursorController.CenterCursorPosition(State.Graphics.Viewport);
                skipNextMouseMoveEvent = true;
                needToCenterMouseBeforeNextMove = false;
            }

            State.Graphics.Viewport.Focus();
        }
    }
}
