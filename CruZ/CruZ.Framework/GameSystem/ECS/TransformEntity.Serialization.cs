using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CruZ.Framework.Serialization;

using Microsoft.Xna.Framework;
using System;

namespace CruZ.Framework.GameSystem.ECS
{
    public partial class TransformEntity : ICustomSerializable
    {
        public event Action? DeserializationCompleted;

        public object ReadJson(JsonReader reader, JsonSerializer serializer)
        {
            var value = ECSManager.CreateTransformEntity();

            JObject jObject;

            jObject = JObject.Load(reader);

            value.Transform.Position = jObject["position"].ToObject<Vector2>(serializer);
            value.Transform.Scale = jObject["scale"].ToObject<Vector2>(serializer);

            foreach (var comJObject in jObject["components"])
            {
                var comRawType = comJObject["com-type"].Value<string>();

                var comTy = Type.GetType(comRawType, GameContext.AssemblyResolver, null) ?? 
                    throw new JsonSerializationException($"Can't load {comRawType} in current Domain");

                var com = (Component)comJObject["com-data"].ToObject(comTy, serializer) ??
                    throw new JsonSerializationException($"Can't deserialize com-data to type {comTy}");

                value.AddComponent(com);
            }

            value.DeserializationCompleted?.Invoke();
            return value;
        }

        public void WriteJson(JsonWriter writer, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            {
                writer.WritePropertyName("position");
                serializer.Serialize(writer, Transform.Position);

                writer.WritePropertyName("scale");
                serializer.Serialize(writer, Transform.Scale);

                writer.WritePropertyName("components");
                writer.WriteStartArray();
                {
                    foreach (var com in GetAllComponents())
                    {
                        writer.WriteStartObject();
                        writer.WritePropertyName("com-type");
                        writer.WriteValue(com.GetType().AssemblyQualifiedName);

                        writer.WritePropertyName("com-data");
                        serializer.Serialize(writer, com, com.GetType());
                        writer.WriteEnd();
                    }
                }
                writer.WriteEnd();
            }
            writer.WriteEnd();
        }
    }
}