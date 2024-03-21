using System;

namespace CruZ.Common.ECS
{
    public class DrawLoopEndEventArgs : EventArgs
    {
        public bool KeepDrawing = false;
        public DrawLoopBeginEventArgs BeginArgs;
    }
}
