using Microsoft.Xna.Framework;

using System;

namespace CruZ.Framework.GameSystem
{
    public class TransformEventArgs : EventArgs
    {
        public Vector2 Position;
        public Vector2 Scale;

        public static TransformEventArgs Create(Transform t)
        {
            var args = new TransformEventArgs();
            args.Position = t.Position;
            args.Scale = t.Scale;
            return args;
        }
    }
}
