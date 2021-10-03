using System;
using System.Windows.Input;
using KihonEngine.GameEngine.Graphics;
using System.Windows.Controls;
using KihonEngine.GameEngine.Graphics.Maps;

namespace KihonEngine.GameEngine
{
    public interface IGameEngineController
    {
        void LoadEmptyMap();
        void LoadMap(IMapBuilder mapBuilder);
        void LoadMap(string mapName);
        void LoadMapFromFile(string filepath);
        void SaveMapToFile(string filepath);

        void RegisterIO(ISynchronizedIO input);
        void RemoveIO(ISynchronizedIO input);
        void NotifyIOs();

        UserControl RegisterDefaultGraphicOutput();
        void RegisterGraphicOutput(Func<IGraphicOutput> normalScreenProvider, Func<IGraphicOutput> fullscreenProvider = null);

        void SwitchToNormalScreen();
        void SwitchToFullScreen();
        void SwitchScreen();

        void StopGameLogic();
        void Play();
        void Play(IMapBuilder mapBuilder);
        void Play<TMapBuilder>() where TMapBuilder : class, IMapBuilder, new();
        void SwitchToPlayMode();
        void SwitchToEditorMode();

        void DispatchKeyboardEvent(KeyEventArgs e);
        void DispatchMouseWheelEvent(MouseWheelEventArgs e);
        void DispatchMouseButtonEvent(MouseButtonEventArgs e);
        void DispatchMouseEvent(MouseEventArgs e);
    }
}
