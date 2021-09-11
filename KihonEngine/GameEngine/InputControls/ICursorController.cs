using System.Windows;

namespace KihonEngine.GameEngine.InputControls
{
    public interface ICursorController
    {
        Point CenterCursorPosition(FrameworkElement element);
    }
}
