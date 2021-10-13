using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace KihonEngine.GameEngine.Graphics.Content
{
    public class EmbeddedContentSource : IContentSource
    {
        private Dictionary<string, BitmapImage> _bitmapImageCache = new Dictionary<string, BitmapImage>();
        private Dictionary<string, Bitmap> _bitmapCache = new Dictionary<string, Bitmap>();
        private Assembly _targetAssembly;

        public EmbeddedContentSource()
        {
            _targetAssembly = typeof(EmbeddedContentSource).Assembly;
        }

        public EmbeddedContentSource(Type type)
        {
            _targetAssembly = type.Assembly;
        }

        public string Name => $"embedded:{_targetAssembly.GetName().Name}";

        public string[] GetResources(GraphicContentType contentType)
        {
            var resourceNames = _targetAssembly.GetManifestResourceNames();
            var assemblyName = _targetAssembly.GetName().Name;
            var prefixResourceName = $"{assemblyName}.Content.Images.{GetResourceDirectory(contentType)}.";

            if (resourceNames.Any())
            {
                var results = resourceNames
                 .Where(x => x.StartsWith(prefixResourceName))
                 .Select(x => x.Substring(prefixResourceName.Length))
                 .ToList();

                results.Sort();

                return results.ToArray();
            }

            return new string[] { };
        }

        public BitmapImage Get(GraphicContentType contentType, string resourceName)
        {
            BitmapImage bitmap = null;

            var shortResourceName = $"{GetResourceDirectory(contentType)}.{resourceName}";
            if (!_bitmapImageCache.TryGetValue(shortResourceName, out bitmap))
            {
                var assemblyName = _targetAssembly.GetName().Name;
                using (var stream = _targetAssembly.GetManifestResourceStream($"{assemblyName}.Content.Images.{shortResourceName}"))
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
                var assemblyName = _targetAssembly.GetName().Name;
                using (var stream = _targetAssembly.GetManifestResourceStream($"{assemblyName}.Content.Images.{shortResourceName}"))
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
