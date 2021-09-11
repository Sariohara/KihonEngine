using System.Runtime.InteropServices;
using System.Windows;

namespace KihonEngine.GameEngine.InputControls
{
    public class CursorController : ICursorController
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        public Point CenterCursorPosition(FrameworkElement element)
        {
            var controlPosition = new Point(element.ActualWidth / 2, element.ActualHeight / 2);
            var screenPosition = element.PointToScreen(controlPosition);

            SetCursorPos((int)screenPosition.X, (int)screenPosition.Y);
            GetCursorPos(out var effectivePosition);

            return element.PointFromScreen(effectivePosition);
        }
    }
}
