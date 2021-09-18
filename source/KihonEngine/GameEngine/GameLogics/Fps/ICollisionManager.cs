using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.GameLogics.Fps
{
    public interface ICollisionManager
    {
        Rect3D GetPlayerBox(Point3D cameraPosition);
        CollisionResult DetectCollisions(Rect3D animatedModelBox);
    }
}
