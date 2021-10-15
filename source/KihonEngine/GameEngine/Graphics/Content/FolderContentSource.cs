using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace KihonEngine.GameEngine.Graphics.Content
{
    public class FolderContentSource : IContentSource
    {
        private Dictionary<string, BitmapImage> _bitmapImageCache = new Dictionary<string, BitmapImage>();
        private Dictionary<string, Bitmap> _bitmapCache = new Dictionary<string, Bitmap>();
        private string _baseDirectory;

        public FolderContentSource()
        {
            _baseDirectory = Environment.CurrentDirectory;
        }

        public FolderContentSource(string baseDirectory)
        {
            _baseDirectory = System.IO.Path.GetFullPath(baseDirectory);
        }

        public ContentSourceDescription Description
            => new ContentSourceDescription { Type = "Folder", Path = _baseDirectory };

        public string[] GetResources(GraphicContentType contentType)
        {
            string folder = GetResourceDirectory(contentType);
            var targetDirectory = System.IO.Path.Combine(_baseDirectory, folder);
            if (Directory.Exists(targetDirectory))
            {
                return Directory.GetFiles(targetDirectory, "*.*", SearchOption.AllDirectories)
                    .Where(x => x.EndsWith(".png") || x.EndsWith(".jpg"))
                    .Select(x => x
                        .Substring(targetDirectory.Length)
                        .Trim('\\').Trim('/')
                        .Replace("\\", "/"))
                    .ToArray();
            }

            return new string[] { };
        }

        public Stream GetStream(GraphicContentType contentType, string resourceName)
        {
            string folder = GetResourceDirectory(contentType);
            var fullResourceName = Path.Combine(_baseDirectory, folder, resourceName.Replace("/", "\\"));
            var directory = Environment.CurrentDirectory;
            var path = Path.Combine(directory, fullResourceName);
            if (File.Exists(path))
            {
                return File.OpenRead(path);
            }

            return null;
        }

        public BitmapImage Get(GraphicContentType contentType, string resourceName)
        {
            BitmapImage bitmap = null;

            string folder = GetResourceDirectory(contentType);
            var fullResourceName = System.IO.Path.Combine(_baseDirectory, folder, resourceName.Replace("/", "\\"));
            if (!_bitmapImageCache.TryGetValue(fullResourceName, out bitmap))
            {
                if (File.Exists(fullResourceName))
                {
                    bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri(fullResourceName, UriKind.RelativeOrAbsolute);
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
            var fullResourceName = System.IO.Path.Combine(_baseDirectory, folder, resourceName.Replace("/", "\\"));
            if (!_bitmapCache.TryGetValue(fullResourceName, out bitmap))
            {
                if (File.Exists(fullResourceName))
                {
                    bitmap = new Bitmap(fullResourceName);

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
