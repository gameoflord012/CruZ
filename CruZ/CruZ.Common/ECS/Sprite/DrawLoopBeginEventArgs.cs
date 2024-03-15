using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework;

namespace CruZ.Common.ECS
{
    public class DrawLoopBeginEventArgs : EventArgs
    {
        public Rectangle SourceRectangle;
        public NUM.Vector2 Origin;
        public NUM.Vector2 Position;
        public NUM.Vector2 Scale;
        public Texture2D? Texture;
        public float LayerDepth = 0;
        public bool Skip = false;

        public DRAW.RectangleF GetWorldBounds() // in World Coordinate
        {
            DRAW.RectangleF rect = new();
            rect.Width = SourceRectangle.Width * Scale.X;
            rect.Height = SourceRectangle.Height * Scale.Y;
            rect.Location = new(
                Position.X - rect.Width * Origin.X,
                Position.Y - rect.Height * Origin.Y);

            return rect;
        }

        public Vector2 GetWorldOrigin()
        {
            var worldBounds = GetWorldBounds();
            return new(
                worldBounds.X + worldBounds.Width * Origin.X,
                worldBounds.Y + worldBounds.Height * Origin.Y
            );
        }
    }
}
