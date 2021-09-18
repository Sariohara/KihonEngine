using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KihonEngine.GameEngine.GameLogics.Fps
{
    public class CollisionResult
    {
        public bool HasBodyCollision { get; set; }
        public bool HasFeetCollision { get; set; }
        public bool HasHeadCollision { get; set; }
        public bool HasCollision { get; set; }
        public bool HasFeetOnFloor { get; set; }
        public bool HasFeetInTheVoid { get; set; }
        public bool HasReachSkybox { get; set; }
        public double DeltaYForFeet { get; set; }
        public double AdjustedYForFeet { get; set; }
        public double DeltaYForHead { get; set; }
        public double AdjustedYForHead { get; set; }
    }
}
