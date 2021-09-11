using System.Windows.Controls;

namespace KihonEngine.GameEngine.Graphics
{
    public interface IGraphicOutput : ISynchronizedIO
    {
        void AttachViewport(Viewport3D viewport);
        void DetachViewport(Viewport3D viewport);
    }
}
