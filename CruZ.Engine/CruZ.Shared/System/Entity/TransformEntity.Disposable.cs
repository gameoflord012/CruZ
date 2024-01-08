using CruZ.Components;
using MonoGame.Extended.Entities;
using System;

namespace CruZ.Components
{
    public partial class TransformEntity : IDisposable
    {
        private void Dispose(bool disposing)
        {
            if(!_disposed)
            {
                if(disposing)
                {
                    this.RemoveFromWorld();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(true);
        }

        ~TransformEntity()
        {
            Dispose(false);
        }

        bool _disposed = false;
    }
}