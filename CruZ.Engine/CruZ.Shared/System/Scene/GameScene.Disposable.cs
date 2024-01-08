using System;

namespace CruZ
{
    public partial class GameScene : IDisposable
    {
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if(!_disposed)
            {
                if(disposing)
                {
                    foreach (var e in _entities)
                    {
                        e.Dispose();
                    }
                }

                _disposed = true;
            }
        }
        
        ~GameScene()
        {
            Dispose(false);
        }

        private bool _disposed = false;
    }
}