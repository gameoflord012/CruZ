using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using RectangleF = System.Drawing.RectangleF;
using MonoGame.Aseprite;

namespace CruZ.GameEngine.GameSystem.Render
{
    public struct SpriteDrawArgs
    {
        public SpriteDrawArgs()
        {
            SpriteEffect = SpriteEffects.None;
            LayerDepth = 0;
            Color = Color.White;
            NormalizedOrigin = new Vector2(0.5f, 0.5f);
        }

        public Texture2D? Texture;
        public Rectangle SourceRectangle;
        public Vector2 NormalizedOrigin;
        public Vector2 Position;
        public Vector2 Scale;
        public float Rotation;
        public Color Color;
        public float LayerDepth;
        public SpriteEffects SpriteEffect;
        public bool Skip = false;

        public void Apply(Transform transform)
        {
            Position = transform.Position;
            Scale = transform.Scale;
            Rotation = transform.Rotation;
        }

        public void Apply(Texture2D tex)
        {
            Texture = tex;
            SourceRectangle = tex.Bounds;
        }

        internal void Apply(Sprite sprite)
        {
            Texture = sprite.TextureRegion.Texture;
            SourceRectangle = sprite.TextureRegion.Bounds;
            Color = sprite.Color * sprite.Transparency;
            Rotation = sprite.Rotation;
            Scale = sprite.Scale;
            LayerDepth = sprite.LayerDepth;
        }

        public WorldRectangle GetWorldBounds() // in ECSWorld Coordinate
        {
            WorldRectangle rect = new();

            rect.W = SourceRectangle.Width * Scale.X;
            rect.H = SourceRectangle.Height * Scale.Y;

            Vector2 origin = new(
                rect.W * NormalizedOrigin.X,
                rect.H * NormalizedOrigin.Y);

            rect.X = Position.X - origin.X;
            rect.Y = Position.Y + origin.Y - rect.H;

            return rect;
        }

        public Vector2 GetWorldOrigin()
        {
            var worldRect = GetWorldBounds();
            return new(
                worldRect.X + worldRect.W * NormalizedOrigin.X,
                worldRect.Y + worldRect.H * (1 - NormalizedOrigin.Y)
            );
        }

        public bool IsOutOfScreen(Matrix viewProjectionMat)
        {
            WorldRectangle worldBounds = GetWorldBounds();

            var min = new Vector4(worldBounds.X, worldBounds.Y, 0, 1);
            var max = new Vector4(worldBounds.Right, worldBounds.Top, 0, 1);

            var matrix = viewProjectionMat;

            var minNDC = Vector4.Transform(min, matrix);
            var maxNDC = Vector4.Transform(max, matrix);

            return maxNDC.X < -1 || maxNDC.Y < -1 || minNDC.X > 1 || minNDC.Y > 1;
        }
    }
}
