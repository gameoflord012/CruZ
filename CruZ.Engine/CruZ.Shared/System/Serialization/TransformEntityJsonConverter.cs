using CruZ_Engine.Components;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace CruZ_Engine.Serialization
{
    public class TransformEntityJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TransformEntity);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var type = value.GetType();

            writer.WriteStartObject();
            {
                var transformEntity = (TransformEntity)value;

                writer.WritePropertyName("Transform");
                serializer.Serialize(writer, type.GetProperty("Transform").GetValue(value));

                writer.WritePropertyName("Components");
                writer.WriteStartArray();
                {
                    foreach(var pair in TransformEntity.GetAllComponents(transformEntity))
                    {
                        writer.WriteStartObject();
                        writer.WritePropertyName(pair.Key.ToString());
                        serializer.Serialize(writer, pair.Value, pair.Key);
                        writer.WriteEnd();
                    }
                }
                writer.WriteEnd();
            }
            writer.WriteEnd();
        }
    }
}