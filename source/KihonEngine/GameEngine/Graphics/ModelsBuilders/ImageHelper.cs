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
            using (var stream = assembly.GetManifestResourceStream($"{assemblyName}.GameEngine.Graphics.Images.{shortResourceName}"))
            {
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
            }

            return bitmap;
        }
    }
}
