using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

using CruZ.Framework.GameSystem.ECS;

namespace CruZ.Framework.Serialization
{
    internal class ComponentJsonConverter : JsonConverter<Component>
    {
        public override Component? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var json = JsonNode.Parse(ref reader);

            string componentTypeInStr = json["ComponentType"].GetValue<string>();
            Type componentType = GetTypeFromString(componentTypeInStr);

            return JsonSerializer.Deserialize(json["ComponentData"], componentType, options) as Component;
        }

        private static Type GetTypeFromString(string componentTypeInString)
        {
            return Type.GetType(componentTypeInString, GameContext.AssemblyResolver, null) ??
                                throw new JsonException($"Can't load {componentTypeInString} in current Domain");
        }

        public override void Write(Utf8JsonWriter writer, Component value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            {
                writer.WritePropertyName("ComponentType");
                writer.WriteStringValue(value.GetType().AssemblyQualifiedName);

                writer.WritePropertyName("ComponentData");
                JsonSerializer.Serialize(writer, value, value.GetType(), options);
            }
            writer.WriteEndObject();
        }
    }
}
