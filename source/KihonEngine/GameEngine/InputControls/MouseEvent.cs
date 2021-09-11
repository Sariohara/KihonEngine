using System.Windows.Input;

namespace KihonEngine.GameEngine.InputControls
{
    public class MouseEvent
    {
        public MouseEventType Type;
        public System.Windows.Point Position;
        public int ClickCount;
        public double DX;
        public double DY;
        public double Delta;
        public MouseButtonState LeftButton;
        public MouseButtonState RightButton;
    }
}
