using System;

namespace CruZ
{
    public partial class GameScene : IDisposable
    {
        public void Dispose()
        {
            foreach (var e in _entities)
            {
                e.Dispose();
            }
        }
    }
}