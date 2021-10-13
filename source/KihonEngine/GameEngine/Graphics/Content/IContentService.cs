using System.Drawing;
using System.Windows.Media.Imaging;

namespace KihonEngine.GameEngine.Graphics.Content
{
    public interface IContentService
    {
        void RegisterSource(IContentSource source);
        string[] GetResources(GraphicContentType contentType);
        BitmapImage Get(GraphicContentType contentType, string resourceName);
        Bitmap GetBitmap(GraphicContentType contentType, string resourceName);
    }
}
