using System;
using System.Windows;
using System.Windows.Input;

namespace KihonEngine.GameEngine.GameLogics
{
    public interface IGameLogic
    {
        public abstract string LogicName { get; }
        bool IsRunning { get; }
        event EventHandler StateChanged;
        void Start();
        void Stop();
        void PublishMouseWheel(Point position, MouseButtonState leftButton, MouseButtonState rightButton, double delta);
        void PublishMouseClick(Point position, MouseButtonState leftButton, MouseButtonState rightButton, int clickCount);
        void PublishMouseMove(Point position, MouseButtonState leftButton, MouseButtonState rightButton, double dx, double dy);
        void RegisterKeyboardPressedKey(Key key);
        void RemoveKeyboardPressedKey(Key key);
    }
}
