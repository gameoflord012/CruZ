using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;

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

        //private Dictionary<string, object?> GetPropertiesValue(object obj)
        //{
        //    Dictionary<string, object?> dict = new();

        //    var props = obj.GetType().GetProperties(
        //        BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.GetField);
            
        //    foreach (var prop in props)
        //    {
        //        var jIgnore = prop.GetCustomAttribute(typeof(JsonIgnoreAttribute));
        //        if(jIgnore == null) dict[prop.Name] = prop.GetValue(obj);
        //    }

        //    return dict;
        //}
    }
}