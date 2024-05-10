using System;

namespace CruZ.GameEngine.GameSystem
{
    internal class EntitySystem : IDisposable
    {
        public virtual void OnInitialize()
        {
        }

        internal void InternalUpdate(SystemEventArgs args)
        {
            OnUpdate(args);
        }

        internal void InternalDraw(SystemEventArgs args)
        {
            OnDraw(args);
        }

        internal void AddedInternal(ECSWorld world)
        {
            AttachedWorld = world;
            OnAttached();
        }

        protected virtual void OnAttached()
        {

        }

        protected virtual void OnUpdate(SystemEventArgs args)
        {

        }

        protected virtual void OnDraw(SystemEventArgs args)
        {

        }

        protected ECSWorld AttachedWorld
        { get; private set; }

        public virtual void Dispose() { }
    }
}
