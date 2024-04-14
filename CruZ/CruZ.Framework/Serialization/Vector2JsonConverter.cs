using Microsoft.Xna.Framework;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CruZ.Framework.Serialization
{
    internal class Vector2JsonConverter : JsonConverter<Vector2>
    {
        public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string str = reader.GetString() ?? throw new JsonException();
            string[] splits = str.Split(',');
            Vector2 v2 = new Vector2();

            if (float.TryParse(splits[0], out float x))
            {
                v2.X = x;
            }
            else throw new JsonException();

            if (float.TryParse(splits[1], out float y))
            {
                v2.Y = y;
            }
            else throw new JsonException();

            return v2;
        }

        public override void Write(Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options)
        {
            writer.WriteStringValue($"{value.X}, {value.Y}");
        }
    }
}
