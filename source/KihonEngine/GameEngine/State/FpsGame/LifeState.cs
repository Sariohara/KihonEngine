using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KihonEngine.GameEngine.State.FpsGame
{
    public class LifeState
    {
        public int Life { get; set; }

        public bool IsAlive => Life > 0;

        public LifeState()
        {
            Life = 150;
        }
    }
}
