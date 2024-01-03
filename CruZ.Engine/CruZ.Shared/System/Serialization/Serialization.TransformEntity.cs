using Box2D.NetStandard.Dynamics.World;
using CruZ.Serialization;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;

namespace CruZ.Components
{
    public partial class TransformEntity : ISerializable
    {
        public void ReadJson(JsonReader reader, JsonSerializer serializer) 
        {
            JObject jObject;

            jObject = JObject.Load(reader);

            Transform.Position = jObject["position"].ToObject<Vector3>(serializer);
            Transform.Scale = jObject["scale"].ToObject<Vector3>(serializer);

            foreach (var com in jObject["components"])
            {
                var comTy = Type.GetType(com["com-type"].Value<string>()) ?? throw new("Incorrect type or can't find it"); ;
                object comValue = com["com-data"].ToObject(comTy, serializer);

                AddComponent((IComponent)comValue);
            }
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
                    foreach (var com in GetAllComponents(this))
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

        ISerializable ISerializable.CreateDefault()
        {
            return new TransformEntity(ECS.World.CreateEntity());
        }
    }
}