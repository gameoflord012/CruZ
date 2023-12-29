using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Runtime.Serialization;

namespace CruZ_Engine.Serialization
{
    public class SerializableJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(ISerializable).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var uninitialObject = (ISerializable)FormatterServices.GetUninitializedObject(objectType);
            ISerializable value = uninitialObject.CreateDefault() ?? uninitialObject;

            value.ReadJson(reader, serializer);
            return value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var serializable = (ISerializable)value;
            serializable.WriteJson(writer, serializer);
        }
    }
}