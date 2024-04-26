using CruZ.GameEngine.GameSystem;

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CruZ.GameEngine.Serialization
{
    internal class TransformEntityJsonConverter : JsonConverter<TransformEntity>
    {
        public override TransformEntity? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            TransformEntity entity = ECSManager.CreateEntity();
            var resolver = options.ReferenceHandler!.CreateResolver();

            using (JsonDocument document = JsonDocument.ParseValue(ref reader))
            {
                JsonElement root = document.RootElement;

                if (root.TryGetProperty("$id", out JsonElement idNode))
                {
                    var refId = idNode.GetString()!;
                    resolver.AddReference(refId, entity);

                    entity.Name = root.GetProperty(nameof(entity.Name)).GetString()!;

                    var parentNode = root.GetProperty(nameof(entity.Parent));
                    entity.Parent = parentNode.Deserialize<TransformEntity?>(options);

                    var transformNode = root.GetProperty(nameof(entity.Transform));
                    entity.Transform = transformNode.Deserialize<Transform>(options)!;

                    var componentsNode = root.GetProperty(nameof(entity.Components));
                    foreach (var componentNode in componentsNode.EnumerateArray())
                    {
                        var component = componentNode.Deserialize<Component>(options)!;
                        entity.AddComponent(component);
                    }
                }
                else if (root.TryGetProperty("$ref", out JsonElement refNode))
                {
                    var refId = refNode.GetString()!;
                    entity = (TransformEntity)resolver.ResolveReference(refId);
                }
            }

            return entity;
        }

        public override void Write(Utf8JsonWriter writer, TransformEntity value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            {
                string refId = options.ReferenceHandler!.CreateResolver().GetReference(value, out bool alreadyExists);

                if (alreadyExists)
                {
                    writer.WriteString("$ref", refId);
                }
                else
                {
                    writer.WriteString("$id", refId);

                    writer.WritePropertyName(nameof(value.Name));
                    writer.WriteStringValue(value.Name);

                    writer.WritePropertyName(nameof(value.Parent));
                    JsonSerializer.Serialize(writer, value.Parent, options);

                    writer.WritePropertyName(nameof(value.Transform));
                    JsonSerializer.Serialize(writer, value.Transform, options);

                    writer.WritePropertyName(nameof(value.Components));

                    writer.WriteStartArray();
                    {
                        foreach (var component in value.Components) JsonSerializer.Serialize(writer, component, options);
                    }
                    writer.WriteEndArray();
                }
            }
            writer.WriteEndObject();
        }
    }
}
