using System;

namespace CruZ.Framework.GameSystem.Render
{
    public class DrawLoopEndEventArgs : EventArgs
    {
        public bool KeepDrawing = false;
        public DrawLoopBeginEventArgs BeginArgs;
    }
}
