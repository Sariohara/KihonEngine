
namespace KihonEngine.GameEngine.State.FpsGame
{
    public class FallState
    {
        public bool IsFalling { get; set; }

        public int FallHeigh { get; set; }

        public bool CanMoveWhenFall { get; set; }

        public bool DeadFall { get; set; }
    }
}
