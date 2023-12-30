using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace CruZ.Serialization
{
    public class SerializableJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(ISerializable).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var uninitialObject = (ISerializable)RuntimeHelpers.GetUninitializedObject(objectType);
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