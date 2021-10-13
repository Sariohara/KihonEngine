using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace KihonEngine.GameEngine.Graphics.Content
{
    public class ContentService : IContentService
    {
        private List<IContentSource> _sources;
        public ContentService()
        {
            _sources = new List<IContentSource>();
        }

        public void RegisterSource(IContentSource source)
        {
            if (!_sources.Contains(source))
            {
                _sources.Add(source);
            }
        }

        public string[] GetSources()
        {
            return _sources.Select(x => x.Name).ToArray();
        }

        public void RemoveSource(string sourceName)
        {
            for(int i = 0; i < _sources.Count(); i++)
            {
                if (_sources[i].Name == sourceName)
                {
                    _sources.RemoveAt(i);
                    break;
                }
            }
        }

        public string[] GetResources(GraphicContentType contentType)
        {
            var result = new List<string>();

            foreach(var source in _sources)
            {
                result.AddRange(source.GetResources(contentType));
            }

            return result.ToArray();
        }

        public BitmapImage Get(GraphicContentType contentType, string resourceName)
        {
            foreach (var source in _sources)
            {
                var result = source.Get(contentType, resourceName);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public Bitmap GetBitmap(GraphicContentType contentType, string resourceName)
        {
            foreach (var source in _sources)
            {
                var result = source.GetBitmap(contentType, resourceName);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}
