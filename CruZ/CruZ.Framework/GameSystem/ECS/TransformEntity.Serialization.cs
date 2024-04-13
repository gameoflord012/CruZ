using CruZ.Framework.Serialization;

using Microsoft.Xna.Framework;
using System;

namespace CruZ.Framework.GameSystem.ECS
{
    public partial class TransformEntity
    {
        //public object ReadJson(JsonReader reader, JsonSerializer serializer)
        //{
        //    var value = ECSManager.CreateTransformEntity();

        //    JObject jObject = JObject.Load(reader);

        //    value.Parent = jObject["Parent"].ToObject<TransformEntity?>(serializer);
        //    value.Transform.Position = jObject["Position"].ToObject<Vector2>(serializer);
        //    value.Transform.Scale = jObject["Scale"].ToObject<Vector2>(serializer);

        //    foreach (var comJObject in jObject["Components"])
        //    {
        //        var comRawType = comJObject["ComponentType"].Value<string>();

        //        var comTy = Type.GetType(comRawType, GameContext.AssemblyResolver, null) ?? 
        //            throw new JsonSerializationException($"Can't load {comRawType} in current Domain");

        //        var com = (Component)comJObject["ComponentData"].ToObject(comTy, serializer) ??
        //            throw new JsonSerializationException($"Can't deserialize com-data to type {comTy}");

        //        value.AddComponent(com);
        //    }

        //    return value;
        //}

        //public void WriteJson(JsonWriter writer, JsonSerializer serializer)
        //{
        //    writer.WriteStartObject();
        //    {
        //        writer.WritePropertyName("Parent");
        //        serializer.Serialize(writer, Parent);

        //        writer.WritePropertyName("Position");
        //        serializer.Serialize(writer, Transform.Position);

        //        writer.WritePropertyName("Scale");
        //        serializer.Serialize(writer, Transform.Scale);

        //        writer.WritePropertyName("Components");
        //        writer.WriteStartArray();
        //        {
        //            foreach (var com in GetAllComponents())
        //            {
        //                writer.WriteStartObject();
        //                writer.WritePropertyName("ComponentType");
        //                writer.WriteValue(com.GetType().AssemblyQualifiedName);

        //                writer.WritePropertyName("ComponentData");
        //                serializer.Serialize(writer, com, com.GetType());
        //                writer.WriteEnd();
        //            }
        //        }
        //        writer.WriteEnd();
        //    }
        //    writer.WriteEnd();
        //}
    }
}