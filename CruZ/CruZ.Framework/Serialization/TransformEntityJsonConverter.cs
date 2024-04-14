using CruZ.Framework.GameSystem.ECS;
using CruZ.Framework.Scene;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CruZ.Framework.Serialization
{
    internal class TransformEntityJsonConverter : JsonConverter<TransformEntity>
    {
        public override TransformEntity? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            TransformEntity entity = ECSManager.CreateTransformEntity();
            var converter = JsonSerializerOptions.Default.GetConverter(typeof(TransformEntity));
            JsonSerializer.P
            return entity;
        }

        public override void Write(Utf8JsonWriter writer, TransformEntity value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            {

            }
            writer.WriteEndObject();
            JsonSerializer.Serialize(writer, value, typeof(TransformEntity), options);
        }
    }
}
