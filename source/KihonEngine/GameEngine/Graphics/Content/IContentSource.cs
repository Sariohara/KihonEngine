using System.Drawing;
using System.Windows.Media.Imaging;

namespace KihonEngine.GameEngine.Graphics.Content
{
    public interface IContentSource
    {
        ContentSourceDescription Description { get; }
        string[] GetResources(GraphicContentType contentType);
        BitmapImage Get(GraphicContentType contentType, string resourceName);
        Bitmap GetBitmap(GraphicContentType contentType, string resourceName);
    }
}
