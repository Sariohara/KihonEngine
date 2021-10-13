using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace KihonEngine.GameEngine.Graphics.Content
{
    public class FileContentSource : IContentSource
    {
        private Dictionary<string, BitmapImage> _bitmapImageCache = new Dictionary<string, BitmapImage>();
        private Dictionary<string, Bitmap> _bitmapCache = new Dictionary<string, Bitmap>();
        private string _baseDirectory;

        public FileContentSource()
        {
            _baseDirectory = Environment.CurrentDirectory;
        }

        public FileContentSource(string baseDirectory)
        {
            _baseDirectory = Path.GetFullPath(baseDirectory);
        }

        public string Name => $"file:{_baseDirectory}";

        public string[] GetResources(GraphicContentType contentType)
        {
            string folder = GetResourceDirectory(contentType);
            var targetDirectory = Path.Combine(_baseDirectory, folder);
            if (Directory.Exists(targetDirectory))
            {
                return Directory.GetFiles(targetDirectory, "*.*", SearchOption.AllDirectories)
                    .Where(x => x.EndsWith(".png") || x.EndsWith(".jpg"))
                    .Select(x => x.ToLower().Replace(targetDirectory.ToLower(), string.Empty).Trim('\\').Trim('/'))
                    .ToArray();
            }

            return new string[] { };
        }

        public BitmapImage Get(GraphicContentType contentType, string resourceName)
        {
            BitmapImage bitmap = null;

            string folder = GetResourceDirectory(contentType);
            var fullResourceName = Path.Combine(_baseDirectory, folder, resourceName);
            if (!_bitmapImageCache.TryGetValue(fullResourceName, out bitmap))
            {
                var directory = Environment.CurrentDirectory;
                var path = Path.Combine(directory, fullResourceName);
                if (File.Exists(path))
                {
                    bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
                    bitmap.EndInit();

                    _bitmapImageCache.Add(fullResourceName, bitmap);
                }
            }

            return bitmap;
        }

        public Bitmap GetBitmap(GraphicContentType contentType, string resourceName)
        {
            Bitmap bitmap = null;

            string folder = GetResourceDirectory(contentType);
            var fullResourceName = Path.Combine(_baseDirectory, folder, resourceName);
            if (!_bitmapCache.TryGetValue(fullResourceName, out bitmap))
            {
                var directory = Environment.CurrentDirectory;
                var path = Path.Combine(directory, fullResourceName);
                if (File.Exists(path))
                {
                    bitmap = new Bitmap(path);

                    _bitmapCache.Add(fullResourceName, bitmap);
                }
            }

            return bitmap;
        }

        private string GetResourceDirectory(GraphicContentType contentType)
        {
            string folder;
            if (contentType == GraphicContentType.Texture)
            {
                folder = "textures";
            }
            else if (contentType == GraphicContentType.Skybox)
            {
                folder = "skyboxes";
            }
            else
            {
                folder = "misc";
            }

            return folder;
        }
    }
}
