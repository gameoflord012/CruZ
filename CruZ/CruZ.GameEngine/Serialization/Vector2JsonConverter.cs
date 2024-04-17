using Microsoft.Xna.Framework;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
namespace CruZ.Framework.Serialization
{
    internal class Vector2JsonConverter : JsonConverter<Vector2>
    {
        public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Vector2 v2 = new();

            using(var document = JsonDocument.ParseValue(ref reader))
            {
                var root = document.RootElement;

                v2.X = root.GetProperty("X").GetSingle();
                v2.Y = root.GetProperty("Y").GetSingle();
            }

            return v2;
        }

        public override void Write(Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            {
                writer.WritePropertyName("X");
                writer.WriteNumberValue(value.X);

                writer.WritePropertyName("Y");
                writer.WriteNumberValue(value.Y);
            }
            writer.WriteEndObject();
        }
    }
}
