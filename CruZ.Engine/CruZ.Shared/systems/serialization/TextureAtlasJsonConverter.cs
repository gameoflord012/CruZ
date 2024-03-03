using System;

using CruZ.Resource;

using Microsoft.Xna.Framework.Graphics;

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

            [JsonIgnore]
            public Guid TextureGuid => new Guid(_textureGuid);

            [JsonProperty(PropertyName = "textureGuid")]
            private string _textureGuid { get; set; }
        }

        public TextureAtlasJsonConverter(ResourceManager resource)
        {
            _resource = resource;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.ValueType == typeof(string))
            {
                throw new NotImplementedException();
            }
            else
            {
                var inlineAtlas = serializer.Deserialize<InlineTextureAtlas>(reader);
                var resInfo = _resource.PreLoad(inlineAtlas.TextureGuid.ToString());
                var texture = _resource.Load<Texture2D>(resInfo);
                return TextureAtlas.Create(texture, inlineAtlas.RegionWidth, inlineAtlas.RegionHeight);
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TextureAtlas);
        }

        ResourceManager _resource;
    }
}
