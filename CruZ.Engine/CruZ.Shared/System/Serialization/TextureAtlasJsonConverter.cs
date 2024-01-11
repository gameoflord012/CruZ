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
            public Guid TextureGuid { get; set; }
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
                var inlineAtlas = serializer.Deserialize<InlineTextureAtlas>(reader);

                var dir = Path.GetDirectoryName(inlineAtlas.Texture);
                var fileName = Path.GetFileNameWithoutExtension(inlineAtlas.Texture);

                var textureProjectPath = Path.Combine(dir, fileName);
                

                Texture2D texture;
                texture = ResourceManager.LoadResource<Texture2D>(inlineAtlas.TextureGuid);
                return TextureAtlas.Create(textureProjectPath, texture, inlineAtlas.RegionWidth, inlineAtlas.RegionHeight);
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TextureAtlas);
        }
    }
}
