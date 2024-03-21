using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework;

namespace CruZ.Common.ECS
{
    public class DrawLoopBeginEventArgs : EventArgs
    {
        public Rectangle SourceRectangle;
        public Vector2 NormalizedOrigin;
        public Vector2 Position;
        public Vector2 Scale;
        public Texture2D? Texture;
        public float LayerDepth = 0;
        public bool Skip = false;

        public DRAW.RectangleF GetWorldBounds() // in World Coordinate
        {
            DRAW.RectangleF rect = new();
            rect.Width = SourceRectangle.Width * Scale.X;
            rect.Height = SourceRectangle.Height * Scale.Y;
            rect.Location = new(
                Position.X - rect.Width * NormalizedOrigin.X,
                Position.Y - rect.Height * NormalizedOrigin.Y);

            return rect;
        }

        public Vector2 GetWorldOrigin()
        {
            var worldBounds = GetWorldBounds();
            return new(
                worldBounds.X + worldBounds.Width * NormalizedOrigin.X,
                worldBounds.Y + worldBounds.Height * NormalizedOrigin.Y
            );
        }
    }
}
