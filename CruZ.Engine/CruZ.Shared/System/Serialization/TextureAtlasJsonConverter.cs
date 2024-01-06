using System;
using System.IO;
using CruZ.Resource;
using CruZ.Utility;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.TextureAtlases;
using Newtonsoft.Json;

namespace CruZ.Serialization
{
    public class TextureAtlasJsonConverter : JsonConverter
    {
        private class InlineTextureAtlas
        {
            public string Texture { get; set; }
            public int RegionWidth { get; set; }
            public int RegionHeight { get; set; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.ValueType == typeof(string))
            {
                //var textureAtlasAssetName = reader.Value.ToString();
                //var contentPath = GetContentPath(textureAtlasAssetName);
                //var texturePackerFile = _contentManager.Load<TexturePackerFile>(contentPath, new JsonContentLoader());
                //var texture = _contentManager.Load<Texture2D>(texturePackerFile.Metadata.Image);
                //return TextureAtlas.Create(texturePackerFile.Metadata.Image, texture );
                throw new NotImplementedException();
            }
            else
            {
                var metadata = serializer.Deserialize<InlineTextureAtlas>(reader);

                // NOTODO: When we get to .NET Standard 2.1 it would be more robust to use
                // [Path.GetRelativePath](https://docs.microsoft.com/en-us/dotnet/api/system.io.path.getrelativepath?view=netstandard-2.1)
                //var directory = Path.GetDirectoryName(_path);

                var dir = Path.GetDirectoryName(metadata.Texture);
                var fileName = Path.GetFileNameWithoutExtension(metadata.Texture);

                var textureProjectPath = Path.Combine(dir, fileName);
                

                Texture2D texture;
                texture = ResourceManager.LoadResource<Texture2D>(
                        Path.GetRelativePath(ResourceManager.GetFullPath(),
                        Helper.ConvertToBinPath(textureProjectPath)));

                return TextureAtlas.Create(textureProjectPath, texture, metadata.RegionWidth, metadata.RegionHeight);

                //try
                //{
                //    texture = ResourceManager.LoadResource<Texture2D>(
                //        Path.GetRelativePath(resolvedAssetName, ResourceManager.GetFullPath()));
                //}
                //catch (Exception ex) {
                //    if (textureDirectory == null || textureDirectory == "") 
                //        texture = ResourceManager.LoadResource<Texture2D>(textureName);
                //    else
                //        texture = ResourceManager.LoadResource<Texture2D>(textureDirectory + "/" + textureName);
                //}
            }
        }

        //private string GetContentPath(string relativePath)
        //{
        //    var directory = Path.GetDirectoryName(_path);
        //    return Path.Combine(directory, relativePath);
        //}

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TextureAtlas);
        }
    }
}
