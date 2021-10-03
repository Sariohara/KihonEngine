
using KihonEngine.GameEngine.Graphics.Maps;
using KihonEngine.GameEngine.Graphics.ModelsBuilders;
using System.Windows;
using System.Windows.Media.Media3D;

namespace KihonEngine.GameEngine.Graphics
{
    public interface IWorldEngine
    {
        string[] RegisteredMaps { get; }

        void RegisterMap(IMapBuilder mapBuilder);
        string RegisterMap<TMapBuilder>() where TMapBuilder : class, IMapBuilder, new();
        void LoadMap(IMapBuilder mapBuilder);
        void LoadMap(string mapName);

        LayeredModel3D GetModel(Point position);
        LayeredModel3D GetModel(Point position, out Point3D? hitPoint);
        LayeredModel3D GetModel(Point position, out Point3D? hitPoint, out Model3D modelHit);

        void AddModel(LayeredModel3D model);
        void RemoveModel(LayeredModel3D model);
        void ReplaceModel(LayeredModel3D oldModel, LayeredModel3D newModel);
    }
}
