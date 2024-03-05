using CruZ.ECS;
using CruZ.GameSystem;
using MonoGame.Extended.Entities;
using System;

namespace CruZ.ECS
{
    public partial class TransformEntity : IDisposable
    {
        public void Dispose()
        {
            this.RemoveFromWorld();
        }

        private void RemoveFromWorld()
        {
            SetIsActive(false);
            GameSystem.ECS.Destroy(_entity);
            RemoveFromWorldEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}