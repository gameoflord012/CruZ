using System;
using System.Text.Json.Serialization;

using CruZ.GameEngine.Serialization;

namespace CruZ.GameEngine.GameSystem
{
    /// <summary>
    /// Base class for Components, every component should have non-parameter constructor
    /// </summary>
    [JsonConverter(typeof(ComponentJsonConverter))]
    public abstract class Component : IDisposable
    {
        protected virtual void OnAttached(TransformEntity entity) { }

        protected virtual void OnDetached(TransformEntity entity) { }

        protected virtual void OnComponentChanged(ComponentCollection comps) { }

        internal void InternalOnAttached(TransformEntity e)
        {
            _attachedEntity = e;
            OnAttached(e);
            e.ComponentsChanged += Entity_ComponentChanged;
        }

        //internal void InternalOnDetached(TransformEntity e)
        //{
        //    _attachedEntity = null;
        //    OnDetached(e);
        //    e.ComponentsChanged -= Entity_ComponentChanged;
        //}

        private void Entity_ComponentChanged(ComponentCollection comps)
        {
            OnComponentChanged(comps);
        }

        public override string ToString()
        {
            return GetType().Name;
        }

        public virtual void Dispose()
        {
        }

        [JsonIgnore]
        public TransformEntity AttachedEntity { get => _attachedEntity ?? throw new InvalidOperationException(); }

        private TransformEntity? _attachedEntity;
    }
}