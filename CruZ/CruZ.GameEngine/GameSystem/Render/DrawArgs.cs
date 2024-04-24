using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using RectangleF = System.Drawing.RectangleF;
using MonoGame.Aseprite;

namespace CruZ.GameEngine.GameSystem.Render
{
    public struct DrawArgs
    {
        public DrawArgs() { }

        public Texture2D? Texture = null;
        public Rectangle SourceRectangle;
        public Vector2 NormalizedOrigin;
        public Vector2 Position;
        public Vector2 Scale;
        public float Rotation;
        public Color Color = Color.White;
        public float LayerDepth = 0;
        public SpriteEffects SpriteEffect = SpriteEffects.None;
        public bool Skip = false;

        public void Apply(TransformEntity entity)
        {
            Position = entity.Transform.Position;
            Scale = entity.Transform.Scale;
            Rotation = entity.Transform.Rotation;
        }

        public void Apply(Texture2D tex)
        {
            Texture = tex;
            SourceRectangle = tex.Bounds;
        }

        public void Apply(Sprite sprite)
        {
            Texture = sprite.TextureRegion.Texture;
            SourceRectangle = sprite.TextureRegion.Bounds;
            Color = sprite.Color * sprite.Transparency;
            Rotation = sprite.Rotation;
            Scale = sprite.Scale;
            LayerDepth = sprite.LayerDepth;
        }

        public RectangleF GetWorldBounds() // in World Coordinate
        {
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
