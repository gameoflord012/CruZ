using Newtonsoft.Json;

using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CruZ.Framework.Serialization
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
            var defaultConstructor = objectType.GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, []) ?? 
                throw new ArgumentException($"{objectType} need to provide a default constructor");

            var customSerializable = (ICustomSerializable)defaultConstructor.Invoke([]);
            var value = customSerializable.ReadJson(reader, serializer);

            return value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var serializable = (ICustomSerializable)value;
            serializable.WriteJson(writer, serializer);
        }
    }
}