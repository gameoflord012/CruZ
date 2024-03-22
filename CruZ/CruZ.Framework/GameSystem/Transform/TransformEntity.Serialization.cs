using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Reflection;

namespace CruZ.Common.ECS
{
    using CruZ.Common.Serialization;

    using Microsoft.Xna.Framework;

    public partial class TransformEntity : ICustomSerializable
    {
        public event Action? DeserializationCompleted;

        public object ReadJson(JsonReader reader, JsonSerializer serializer)
        {
            var value = ECSManager.CreateEntity();

            JObject jObject;

            jObject = JObject.Load(reader);

            value.Transform.Position = jObject["position"].ToObject<Vector2>(serializer);
            value.Transform.Scale = jObject["scale"].ToObject<Vector2>(serializer);

            foreach (var comObject in jObject["components"])
            {
                var tyStr = comObject["com-type"].Value<string>();

                var comTy = Type.GetType(tyStr, (assName) =>
                {
                    return Assembly.Load(assName);
                }, null) ?? throw new(string.Format("Can't get Type from string \"{0}\"", tyStr));

                object comData = comObject["com-data"].ToObject(comTy, serializer);
                var com = (Component)comData;

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