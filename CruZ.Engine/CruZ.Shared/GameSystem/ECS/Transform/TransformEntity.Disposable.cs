using System;

namespace CruZ.Common.ECS
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
            ECSManager.Destroy(_entity);
            RemoveFromWorldEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}