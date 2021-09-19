using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.State.FpsGame
{
    public class JumpState
    {
        public JumpState()
        {
            InitialYSpeed = 2;
            Gravity = .1;
            FallSizeLimitToDeath = 200;
            YSpeedMax = 10;
        }

        public bool IsJumping { get; set; }

        public Vector3D JumpDirection { get; set; }

        public double FallSize { get; set; }

        public double FallSizeLimitToDeath { get; set; }

        public double YSpeed { get; set; }

        public double YSpeedMax { get; set; }

        public int InitialYSpeed { get; set; }

        public double Gravity { get; set; }

        public bool HasMoveForward { get; set; }

        public bool HasMoveBackward { get; set; }

        public bool HasMoveRight { get; set; }

        public bool HasMoveLeft { get; set; }
    }
}
