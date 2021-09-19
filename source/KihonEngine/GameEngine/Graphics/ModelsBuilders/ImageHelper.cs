using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace KihonEngine.GameEngine.Graphics.ModelsBuilders
{
    public static class ImageHelper
    {
        public static BitmapImage Get(string shortResourceName)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var bitmap = new BitmapImage();

            var assemblyName = typeof(ImageHelper).Assembly.GetName().Name;
            using (var stream = assembly.GetManifestResourceStream($"{assemblyName}.Content.Images.{shortResourceName}"))
            {
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
            }

            return bitmap;
        }

        public static Bitmap GetBitmap(string shortResourceName)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var bitmap = new BitmapImage();

            var assemblyName = typeof(ImageHelper).Assembly.GetName().Name;
            using (var stream = assembly.GetManifestResourceStream($"{assemblyName}.Content.Images.{shortResourceName}"))
            {
                return new Bitmap(stream);
            }
        }

        public static BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
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
