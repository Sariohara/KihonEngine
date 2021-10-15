using KihonEngine.GameEngine.Graphics.Content;
using KihonEngine.GameEngine.Graphics.ModelDefinitions;
using KihonEngine.Services;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace KihonEngine.Studio.Services
{
    public class FileContentSourceBuilder
    {
        private IContentService ContentService
            => Container.Get<IContentService>();

        public void Create(string filepath, MapDefinition definition)
        {
            var skyboxes = GetSkyboxes(definition);
            var textures = GetTextures(definition);

            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach(var texture in textures)
                    {
                        var sourceStream = ContentService.GetStream(GraphicContentType.Texture, texture);
                        if (sourceStream != null)
                        {
                            var file = archive.CreateEntry("textures/" + texture);

                            using (var entryStream = file.Open())
                            {
                                sourceStream.CopyTo(entryStream);
                            }

                            sourceStream.Dispose();
                        }
                    }

                    foreach (var skybox in skyboxes)
                    {
                        var sourceStream = ContentService.GetStream(GraphicContentType.Skybox, skybox);
                        if (sourceStream != null)
                        {
                            var file = archive.CreateEntry("skyboxes/" + skybox);

                            using (var entryStream = file.Open())
                            {
                                sourceStream.CopyTo(entryStream);
                            }

                            sourceStream.Dispose();
                        }
                    }
                }

                using (var fileStream = new FileStream(filepath, FileMode.Create))
                {
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    memoryStream.CopyTo(fileStream);
                }
            }
        }

        private string[] GetSkyboxes(MapDefinition definition)
        {
            var skyboxes = new List<string>();
            foreach (var element in definition.Skyboxes)
            {
                AddToList(skyboxes, element.Metadata.Name);
            }

            return skyboxes.ToArray();
        }

        private string[] GetTextures(MapDefinition definition)
        {
            var textures = new List<string>();
            foreach (var element in definition.Ceilings)
            {
                AddToList(textures, element.Metadata.Texture?.Name);
            }

            foreach (var element in definition.Floors)
            {
                AddToList(textures, element.Metadata.Texture?.Name);
            }

            foreach (var element in definition.Volumes)
            {
                AddToList(textures, element.Metadata.TextureBack?.Name);
                AddToList(textures, element.Metadata.TextureBottom?.Name);
                AddToList(textures, element.Metadata.TextureFront?.Name);
                AddToList(textures, element.Metadata.TextureLeft?.Name);
                AddToList(textures, element.Metadata.TextureRight?.Name);
                AddToList(textures, element.Metadata.TextureTop?.Name);
            }

            foreach (var element in definition.Walls)
            {
                AddToList(textures, element.Metadata.Texture?.Name);
            }

            return textures.ToArray();
        }

        private void AddToList(List<string> target, string element)
        {
            if (!string.IsNullOrEmpty(element) && !target.Contains(element))
            {
                target.Add(element);
            }
        }
    }
}
