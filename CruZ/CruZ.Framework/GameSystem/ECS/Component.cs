using System;
using System.Text.Json.Serialization;

using CruZ.Framework.Serialization;

namespace CruZ.Framework.GameSystem.ECS
{
    [JsonConverter(typeof(ComponentJsonConverter))]
    public abstract class Component : IDisposable
    {
        protected virtual void OnAttached(TransformEntity entity) { }

        protected virtual void OnDetached(TransformEntity entity) { }

        protected virtual void OnComponentChanged(ComponentCollection comps) { }

        internal void InternalOnAttached(TransformEntity e)
        {
            AttachedEntity = e;
            OnAttached(e);
            e.ComponentsChanged += Entity_ComponentChanged;
        }

        internal void InternalOnDetached(TransformEntity e)
        {
            AttachedEntity = null;
            OnDetached(e);
            e.ComponentsChanged -= Entity_ComponentChanged;
        }

        private void Entity_ComponentChanged(ComponentCollection comps)
        {
            OnComponentChanged(comps);
        }

        public override string ToString()
        {
            return GetType().Name;
        }

        public void Dispose()
        {
            OnDispose();
        }

        protected virtual void OnDispose()
        {

        }

        //public object ReadJson(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        //{
        //    throw new NotImplementedException();
        //}

        //public void WriteJson(Utf8JsonWriter writer, IJsonSerializable value, JsonSerializerOptions options)
        //{


        //    writer.WriteStartObject();
        //    {
        //        writer.WritePropertyName("ComponentType");
        //        writer.WriteStringValue(this.GetType().FullName);

        //        writer.WritePropertyName("ComponentData");
        //        var converter = options.GetConverter(this.GetType());
        //        JsonSerializer.Serialize(this, )
        //    }
        //    writer.WriteEndObject();
        //}

        [JsonIgnore]
        protected TransformEntity? AttachedEntity { get; private set; }
    }
}