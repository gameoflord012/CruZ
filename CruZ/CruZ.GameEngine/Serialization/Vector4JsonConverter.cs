using Microsoft.Xna.Framework;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CruZ.GameEngine.Serialization
{
    internal class Vector4JsonConverter : JsonConverter<Vector4>
    {
        public override Vector4 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Vector4 v4 = new();

            using (var document = JsonDocument.ParseValue(ref reader))
            {
                var root = document.RootElement;

                v4.X = root.GetProperty("X").GetSingle();
                v4.Y = root.GetProperty("Y").GetSingle();
                v4.Z = root.GetProperty("Z").GetSingle();
                v4.W = root.GetProperty("W").GetSingle();
            }

            return v4;
        }

        public override void Write(Utf8JsonWriter writer, Vector4 value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            {
                writer.WritePropertyName("X");
                writer.WriteNumberValue(value.X);

                writer.WritePropertyName("Y");
                writer.WriteNumberValue(value.Y);

                writer.WritePropertyName("Z");
                writer.WriteNumberValue(value.Z);

                writer.WritePropertyName("W");
                writer.WriteNumberValue(value.W);
            }
            writer.WriteEndObject();
        }
    }
}
