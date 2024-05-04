using System;

namespace CruZ.GameEngine.GameSystem
{
    internal class EntitySystem : IDisposable
    {
        public virtual void OnInitialize() { }

        internal void DoUpdate(EntitySystemEventArgs args)
        {
            OnUpdate(args);
        }

        internal void DoDraw(EntitySystemEventArgs args)
        {
            OnDraw(args);
        }

        protected virtual void OnUpdate(EntitySystemEventArgs args)
        {

        }

        protected virtual void OnDraw(EntitySystemEventArgs args)
        {

        }

        public virtual void Dispose() { }
    }
}
