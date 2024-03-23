using System;

namespace CruZ.Framework.GameSystem.Render
{
    public class DrawLoopEndEventArgs : EventArgs
    {
        public bool KeepDrawing = false;
        public DrawArgs DrawArgs { get; }

        public DrawLoopEndEventArgs(DrawArgs drawArgs)
        {
            DrawArgs = drawArgs;
        }
    }
}
