using Microsoft.Xna.Framework;

using System;

namespace CruZ.Common.ECS
{
    public class TransformEventArgs : EventArgs
    {
        public Vector3 Position;
        public Vector3 Scale;

        public static TransformEventArgs Create(Transform t)
        {
            var args = new TransformEventArgs();
            args.Position = t.Position;
            args.Scale = t.Scale;
            return args;
        }
    }
}
