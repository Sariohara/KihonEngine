using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace KihonEngine.GameEngine.Graphics.Content
{
    public class EmbeddedContentSource : IContentSource
    {
        private Dictionary<string, BitmapImage> _bitmapImageCache = new Dictionary<string, BitmapImage>();
        private Dictionary<string, Bitmap> _bitmapCache = new Dictionary<string, Bitmap>();

        public string[] GetResources(GraphicContentType contentType)
        {
            if (contentType == GraphicContentType.Texture)
            {
                return new[] {
                    "default.png",
                    "floor0.jpg",
                    "ki.png",
                    "hon.png",
                    "steve-front.png",
                    "steve-back.png",
                    "steve-top.png",
                    "steve-left.png",
                    "steve-right.png",
                };
            }

            if (contentType == GraphicContentType.Skybox)
            {
                return new[] {
                    "sky0-full.png",
                    "sky1-full.png",
                    "sky2-full.png",
                    "sky3-full.png",
                };
            }

            return new string[] { };
        }

        public BitmapImage Get(GraphicContentType contentType, string resourceName)
        {
            BitmapImage bitmap = null;

            var shortResourceName = $"{GetResourceDirectory(contentType)}.{resourceName}";
            if (!_bitmapImageCache.TryGetValue(shortResourceName, out bitmap))
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();

                var assemblyName = typeof(EmbeddedContentSource).Assembly.GetName().Name;
                using (var stream = assembly.GetManifestResourceStream($"{assemblyName}.Content.Images.{shortResourceName}"))
                {
                    if (stream != null)
                    {
                        bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = stream;
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        _bitmapImageCache.Add(shortResourceName, bitmap);
                    }
                }
            }

            return bitmap;
        }

        public Bitmap GetBitmap(GraphicContentType contentType, string resourceName)
        {
            Bitmap bitmap = null;

            var shortResourceName = $"{GetResourceDirectory(contentType)}.{resourceName}";
            if (!_bitmapCache.TryGetValue(shortResourceName, out bitmap))
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();

                var assemblyName = typeof(EmbeddedContentSource).Assembly.GetName().Name;
                using (var stream = assembly.GetManifestResourceStream($"{assemblyName}.Content.Images.{shortResourceName}"))
                {
                    if (stream != null)
                    {
                        bitmap = new Bitmap(stream);
                        _bitmapCache.Add(shortResourceName, bitmap);
                    }
                }
            }

            return bitmap;
        }

        private string GetResourceDirectory(GraphicContentType contentType)
        {
            string folder;
            if (contentType == GraphicContentType.Texture)
            {
                folder = "Textures";
            }
            else if (contentType == GraphicContentType.Skybox)
            {
                folder = "Skyboxes";
            }
            else
            {
                folder = "Misc";
            }

            return folder;
        }
    }
}
