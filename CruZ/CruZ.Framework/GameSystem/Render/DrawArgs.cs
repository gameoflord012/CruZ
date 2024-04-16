using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework;

using RectangleF = System.Drawing.RectangleF;

namespace CruZ.Framework.GameSystem.Render
{
    public class DrawArgs : EventArgs
    {
        public Texture2D? Texture;
        public Rectangle SourceRectangle;
        public Vector2 NormalizedOrigin = new(0.5f, 0.5f);
        public Vector2 Position;
        public Vector2 Scale;
        public Color Color = Color.White;
        public float LayerDepth = 0;
        public bool Flip = false;
        public bool Skip = false;

        public void Apply(TransformEntity entity)
        {
            Position = entity.Position;
            Scale = entity.Scale;
        }

        public void Apply(Texture2D tex)
        {
            Texture = tex;
            SourceRectangle = tex.Bounds;
        }

        public RectangleF GetWorldBounds() // in World Coordinate
        {
            if(SourceRectangle.IsEmpty) throw new InvalidOperationException("set rect value first");

            RectangleF rect = new();
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
