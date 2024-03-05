using Newtonsoft.Json;

using System;
using System.Runtime.CompilerServices;

namespace CruZ.Serialization
{
    public class SerializableJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(ICustomSerializable).IsAssignableFrom(objectType);
        }

        // TODO: Fix this
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var uninitialObject = (ICustomSerializable)RuntimeHelpers.GetUninitializedObject(objectType);
            var value = uninitialObject.ReadJson(reader, serializer);
            return value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var serializable = (ICustomSerializable)value;
            serializable.WriteJson(writer, serializer);
        }
    }
}