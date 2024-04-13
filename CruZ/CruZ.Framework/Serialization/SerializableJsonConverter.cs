
//using System;
//using System.Reflection;
//using System.Text.Json;
//using System.Text.Json.Serialization;

//namespace CruZ.Framework.Serialization
//{
//    public class SerializableJsonConverter : JsonConverter<IJsonSerializable>
//    {
//        public override bool CanConvert(Type objectType)
//        {
//            return typeof(IJsonSerializable).IsAssignableFrom(objectType);
//        }

//        public override IJsonSerializable? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//        {
//            var defaultConstructor = typeToConvert.GetConstructor(
//                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, []) ??
//                throw new ArgumentException($"{typeToConvert} need to provide a default constructor");

//            var customSerializable = (IJsonSerializable)defaultConstructor.Invoke([]);
//            var value = (IJsonSerializable)customSerializable.ReadJson(ref reader, typeToConvert, options);

//            return value;
//        }

//        public override void Write(Utf8JsonWriter writer, IJsonSerializable value, JsonSerializerOptions options)
//        {
//            var serializable = value;
//            serializable.WriteJson(writer, value, options);
//        }
//    }
//}