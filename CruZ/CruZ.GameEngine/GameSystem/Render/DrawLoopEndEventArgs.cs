using System;

namespace CruZ.GameEngine.GameSystem.Render
{
    public class DrawLoopEndEventArgs : EventArgs
    {
        public bool KeepDrawing = false;
        public DrawSpriteArgs DrawArgs { get; }

        public DrawLoopEndEventArgs(DrawSpriteArgs drawArgs)
        {
            DrawArgs = drawArgs;
        }
    }
}
