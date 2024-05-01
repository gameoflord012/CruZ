using System;

namespace CruZ.GameEngine.GameSystem.Render
{
    public class DrawLoopEndEventArgs : EventArgs
    {
        public bool KeepDrawing = false;
        public SpriteDrawArgs DrawArgs { get; }

        public DrawLoopEndEventArgs(SpriteDrawArgs drawArgs)
        {
            DrawArgs = drawArgs;
        }
    }
}
