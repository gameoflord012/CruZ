using CruZ.Components;
using CruZ.Systems;
using MonoGame.Extended.Entities;
using System;

namespace CruZ.Components
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

            ECS.Destroy(_entity);
            RemoveFromWorldEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}