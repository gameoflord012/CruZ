using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace CruZ.Components
{
    //public interface IComponent
    //{
    //    [Browsable(false)]
    //    Type ComponentType { get; }
    //}

    public abstract class Component
    {
        [Browsable(false)]
        public abstract Type ComponentType { get; }

        public Component() { Initialize(); }

        protected virtual void Initialize() { }
        protected virtual void OnAttached(TransformEntity entity) { }
        protected virtual void OnDetached(TransformEntity entity) { }

        internal void InternalOnAttached(TransformEntity e) { OnAttached(e); }
        internal void InternalOnDetached(TransformEntity e) { OnDetached(e); }
    }
}