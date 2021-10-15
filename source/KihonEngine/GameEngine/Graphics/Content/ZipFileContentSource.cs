using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO.Compression;

namespace KihonEngine.GameEngine.Graphics.Content
{
    public class ZipFileContentSource : IContentSource
    {
        private Dictionary<string, BitmapImage> _bitmapImageCache = new Dictionary<string, BitmapImage>();
        private Dictionary<string, Bitmap> _bitmapCache = new Dictionary<string, Bitmap>();
        private string _zipFilename;

        public ZipFileContentSource(string filename)
        {
            _zipFilename = filename;
        }

        public ContentSourceDescription Description
            => new ContentSourceDescription { Type = "File", Path = _zipFilename };

        public string[] GetResources(GraphicContentType contentType)
        {
            if (File.Exists(_zipFilename))
            {
                var filenames = new List<string>();
                string folder = GetResourceDirectory(contentType);
                using (var zip = ZipFile.Open(_zipFilename, ZipArchiveMode.Read))
                {
                    foreach (ZipArchiveEntry entry in zip.Entries)
                    {
                        if (entry.FullName.StartsWith($"{folder}/"))
                        {
                            filenames.Add(entry.FullName.Substring(folder.Length + 1));
                        }
                    }
                }

                return filenames
                    .Where(x => x.EndsWith(".png") || x.EndsWith(".jpg"))
                    .ToArray();
            }
                
            return new string[] { };
        }

        public Stream GetStream(GraphicContentType contentType, string resourceName)
        {
            string folder = GetResourceDirectory(contentType);
            var fullResourceName = $"{folder}/{resourceName}";
            
            using (var archive = ZipFile.OpenRead(_zipFilename))
            {
                var entry = archive.GetEntry(fullResourceName);
                if (entry != null)
                {
                    using (var zipStream = entry.Open())
                    {
                        var memoryStream = new MemoryStream();
                        zipStream.CopyTo(memoryStream);
                        memoryStream.Position = 0;
                        return memoryStream;
                    }
                }

                return null;
            }
        }

        public BitmapImage Get(GraphicContentType contentType, string resourceName)
        {
            if (!File.Exists(_zipFilename))
            {
                return null;
            }

            BitmapImage bitmap = null;

            string folder = GetResourceDirectory(contentType);
            var fullResourceName = $"{folder}/{resourceName}";
            if (!_bitmapImageCache.TryGetValue(fullResourceName, out bitmap))
            {
                using (var archive = ZipFile.OpenRead(_zipFilename))
                {
                    var entry = archive.GetEntry(fullResourceName);
                    if (entry != null)
                    {
                        using (var zipStream = entry.Open())
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                zipStream.CopyTo(memoryStream);
                                memoryStream.Position = 0;

                                bitmap = new BitmapImage();
                                bitmap.BeginInit();
                                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                bitmap.StreamSource = memoryStream;
                                bitmap.EndInit();

                                _bitmapImageCache.Add(fullResourceName, bitmap);
                            }
                        }
                    }
                }
            }

            return bitmap;
        }

        public Bitmap GetBitmap(GraphicContentType contentType, string resourceName)
        {
            if (!File.Exists(_zipFilename))
            {
                return null;
            }

            Bitmap bitmap = null;

            string folder = GetResourceDirectory(contentType);
            var fullResourceName = $"{folder}/{resourceName}";
            if (!_bitmapCache.TryGetValue(fullResourceName, out bitmap))
            {
                using (var archive = ZipFile.OpenRead(_zipFilename))
                {
                    var entry = archive.GetEntry(fullResourceName);
                    if (entry != null)
                    {
                        using (var zipStream = entry.Open())
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                zipStream.CopyTo(memoryStream);
                                memoryStream.Position = 0;

                                bitmap = new Bitmap(memoryStream);
                                _bitmapCache.Add(fullResourceName, bitmap);
                            }
                        }
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
