//using CruZ.Components;
//using Newtonsoft.Json;
//using System;

//namespace CruZ.Serialization
//{
//    public class TransformJsonConverter : JsonConverter
//    {
//        public override bool CanConvert(Type objectType)
//        {
//            return objectType == typeof(Transform);
//        }

//        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
//        {
//            return serializer.Deserialize(reader);
//        }

//        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
//        {
//            serializer.Serialize(writer, value);
//        }
//    }
//}