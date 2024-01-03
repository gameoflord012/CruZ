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
            IEnumerable<JObject> components = jObject["components"].Cast<JObject>();

            foreach (var comObj in components)
            {
                var comProp = comObj.Properties().First();
                Type comTy = Type.GetType(comProp.Name);
                object comValue = comProp.Value.ToObject(comTy, serializer);

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
                        writer.WritePropertyName(com.GetType().AssemblyQualifiedName);
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