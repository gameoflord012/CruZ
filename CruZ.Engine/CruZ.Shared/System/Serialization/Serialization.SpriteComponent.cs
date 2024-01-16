using CruZ.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace CruZ.Components
{
    public partial class SpriteComponent : ISerializable
    {
        public void ReadJson(JsonReader reader, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            _textureURI = jObject["_resourceName"].Value<string>();
            LayerDepth = jObject["LayerDepth"].Value<float>();
            LoadTexture(_textureURI);
        }

        public void WriteJson(JsonWriter writer, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("_resourceName");
            writer.WriteValue(_textureURI);
            writer.WritePropertyName("LayerDepth");
            writer.WriteValue(LayerDepth);
            writer.WriteEnd();
        }

        ISerializable ISerializable.CreateDefault()
        {
            return new SpriteComponent();
        }
    }
}