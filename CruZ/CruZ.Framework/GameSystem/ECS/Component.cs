using System;

namespace CruZ.Framework.GameSystem.ECS
{
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

        protected TransformEntity? AttachedEntity { get; private set; }
    }
}