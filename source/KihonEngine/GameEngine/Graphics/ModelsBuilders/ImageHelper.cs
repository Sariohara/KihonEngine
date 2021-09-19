using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace KihonEngine.GameEngine.Graphics.ModelsBuilders
{
    public static class ImageHelper
    {
        private static Dictionary<string, BitmapImage>  _bitmapImageCache = new Dictionary<string, BitmapImage>();
        private static Dictionary<string, Bitmap> _bitmapCache = new Dictionary<string, Bitmap>();

        public static BitmapImage Get(string shortResourceName)
        {
            BitmapImage bitmap = null;
            
            if (!_bitmapImageCache.TryGetValue(shortResourceName, out bitmap))
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                bitmap = new BitmapImage();

                var assemblyName = typeof(ImageHelper).Assembly.GetName().Name;
                using (var stream = assembly.GetManifestResourceStream($"{assemblyName}.Content.Images.{shortResourceName}"))
                {
                    bitmap.BeginInit();
                    bitmap.StreamSource = stream;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                }

                _bitmapImageCache.Add(shortResourceName, bitmap);
            }

            return bitmap;
        }

        public static Bitmap GetBitmap(string shortResourceName)
        {
            Bitmap bitmap = null;

            if (!_bitmapCache.TryGetValue(shortResourceName, out bitmap))
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();

                var assemblyName = typeof(ImageHelper).Assembly.GetName().Name;
                using (var stream = assembly.GetManifestResourceStream($"{assemblyName}.Content.Images.{shortResourceName}"))
                {
                    bitmap = new Bitmap(stream);
                }

                _bitmapCache.Add(shortResourceName, bitmap);
            }

            return bitmap;
        }

        public static BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        /// <summary>
        /// Get part of skybox.
        /// </summary>
        /// <param name="shortResourceName">path to resource</param>
        /// <param name="column">column in skybox image</param>
        /// <param name="row">row in skybox image</param>
        /// <returns></returns>
        public static BitmapImage GetSkyboxPart(string shortResourceName, SkyboxFace face)
        {
            BitmapImage bitmap = null;

            var key = $"{shortResourceName}|{face}";
            if (!_bitmapImageCache.TryGetValue(key, out bitmap))
            {
                bitmap = BuildSkyboxPart(shortResourceName, face);
                _bitmapImageCache.Add(key, bitmap);
            }

            return bitmap;
        }
        
        private static BitmapImage BuildSkyboxPart(string shortResourceName, SkyboxFace face)
        {
            var imageSource = GetBitmap(shortResourceName);
            var sizeX = imageSource.Size.Width / 4;
            var sizeY = imageSource.Size.Height / 3;
            int x = 0;
            int y = 0;

            switch (face)
            {
                case SkyboxFace.Front:
                    {
                        x = sizeX * 2;
                        y = sizeY * 1;
                        break;
                    }
                case SkyboxFace.Back:
                    {
                        x = sizeX * 0;
                        y = sizeY * 1;
                        break;
                    }
                case SkyboxFace.Top:
                    {
                        x = sizeX * 2;
                        y = sizeY * 0;
                        break;
                    }
                case SkyboxFace.Bottom:
                    {
                        x = sizeX * 2;
                        y = sizeY * 2;
                        break;
                    }
                case SkyboxFace.Right:
                    {
                        x = sizeX * 3;
                        y = sizeY * 1;
                        break;
                    }
                case SkyboxFace.Left:
                    {
                        x = sizeX * 1;
                        y = sizeY * 1;
                        break;
                    }
            }

            Rectangle cloneRect = new Rectangle(x, y, sizeX, sizeY);
            Bitmap partialImage = imageSource.Clone(cloneRect, imageSource.PixelFormat);

            return ToBitmapImage(partialImage);
        }
    }
}
