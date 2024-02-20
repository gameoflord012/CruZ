using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace CruZ.Components
{
    public abstract class Component
    {
        [Browsable(false)]
        public abstract Type ComponentType { get; }

        public Component() { Initialize(); }

        protected virtual void Initialize() { }
        protected virtual void OnAttached(TransformEntity entity) { }
        protected virtual void OnDetached(TransformEntity entity) { }
        protected virtual void OnComponentChanged(ComponentCollection comps) { }

        internal void InternalOnAttached(TransformEntity e) 
        { 
            OnAttached(e);
            e.ComponentChanged += Entity_ComponentChanged;
        }

        internal void InternalOnDetached(TransformEntity e) 
        { 
            OnDetached(e);
            e.ComponentChanged -= Entity_ComponentChanged;
        }

        private void Entity_ComponentChanged(ComponentCollection comps)
        {
            OnComponentChanged(comps);
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}