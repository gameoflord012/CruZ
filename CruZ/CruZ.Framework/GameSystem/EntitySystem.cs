using System;

namespace CruZ.Framework.GameSystem
{
    internal class EntitySystem : IDisposable
    {
        public virtual void OnInitialize() { }

        public void Update(EntitySystemEventArgs args)
        {
            OnUpdate(args);
        }

        public void Draw(EntitySystemEventArgs args)
        {
            OnDraw(args);
        }

        protected virtual void OnUpdate(EntitySystemEventArgs args)
        {

        }

        protected virtual void OnDraw(EntitySystemEventArgs args)
        {

        }

        public void Dispose()
        {
            OnDispose();
        }

        protected virtual void OnDispose() { }
    }
}
