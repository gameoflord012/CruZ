using CruZ_Engine.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace CruZ_Engine.Components
{
    public partial class SpriteComponent : ISerializable
    {
        public void ReadJson(JsonReader reader, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            _resourceName = jObject["_resourceName"].Value<string>();
            LoadTexture(_resourceName);
        }

        public void WriteJson(JsonWriter writer, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("_resourceName");
            writer.WriteValue(_resourceName);
            writer.WriteEnd();
        }

        ISerializable ISerializable.CreateDefault()
        {
            return new SpriteComponent();
        }
    }
}