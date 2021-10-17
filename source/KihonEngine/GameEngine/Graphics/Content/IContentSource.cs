using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace KihonEngine.GameEngine.Graphics.Content
{
    public interface IContentSource
    {
        ContentSourceDescription Description { get; }
        string[] GetResources(GraphicContentType contentType);
        Stream GetStream(GraphicContentType contentType, string resourceName);
        BitmapImage Get(GraphicContentType contentType, string resourceName);
        Bitmap GetBitmap(GraphicContentType contentType, string resourceName);
    }
}
