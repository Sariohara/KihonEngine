using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KihonEngine.GameEngine.State.FpsGame
{
    public class JumpState
    {
        public JumpState()
        {
            InitialYSpeed = 15;
            Gravity = .2;
        }

        public bool IsJumping { get; set; }

        public double YSpeed { get; set; }

        public int InitialYSpeed { get; set; }

        public double Gravity { get; set; }

        public bool HasMoveForward { get; set; }

        public bool HasMoveBackward { get; set; }

        public bool HasMoveRight { get; set; }

        public bool HasMoveLeft { get; set; }
    }
}
