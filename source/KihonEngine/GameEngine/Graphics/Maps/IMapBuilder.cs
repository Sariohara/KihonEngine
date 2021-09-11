using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.Graphics.Maps
{
    public interface IMapBuilder
    {
        string MapName { get; }

        List<LayeredModel3D> CreateMap();

        Point3D PlayerPosition { get; }

        Vector3D PlayerLookDirection { get; }
    }
}
