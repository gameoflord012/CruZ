using Microsoft.Xna.Framework;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CruZ.Framework.Serialization
{
    internal class Vector4JsonConverter : JsonConverter<Vector4>
    {
        public override Vector4 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string str = reader.GetString() ?? throw new JsonException();
            string[] splits = str.Split(',');
            Vector4 v = new();

            v.X = float.Parse(splits[0]);
            v.Y = float.Parse(splits[1]);
            v.Z = float.Parse(splits[2]);
            v.W = float.Parse(splits[3]);

            return v;
        }

        public override void Write(Utf8JsonWriter writer, Vector4 value, JsonSerializerOptions options)
        {
            writer.WriteStringValue($"{value.X}, {value.Y}, {value.Z}, {value.W}");
        }
    }
}
