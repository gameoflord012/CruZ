using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.TextureAtlases;

namespace CruZ.GameEngine.GameSystem.Render
{

    /// <summary>
    /// Defines extension methods for the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to render graphical resource types in this library.
    /// </summary>
    public static class SpriteBatchHelper
    {
        public static void DrawWorld(this SpriteBatch spriteBatch, SpriteDrawArgs args)
        {
            if(args.Texture == null) return;

            spriteBatch.DrawWorld(
                args.Texture,
                args.Position,
                args.SourceRectangle,
                args.Color,
                args.Rotation,
                new Vector2(args.NormalizedOrigin.X * args.SourceRectangle.Width, args.NormalizedOrigin.Y * args.SourceRectangle.Height),
                args.Scale,
                args.SpriteEffect,
                args.LayerDepth);
        }

        public static void DrawWorld(this SpriteBatch spriteBatch, TextureRegion2D textureRegion, Vector2 position, Color color,
            float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth, Rectangle? clippingRectangle = null)
        {
            var sourceRectangle = textureRegion.Bounds;

            if (clippingRectangle.HasValue)
            {
                var x = (int)(position.X - origin.X);
                var y = (int)(position.Y - origin.Y);
                var width = (int)(textureRegion.Width * scale.X);
                var height = (int)(textureRegion.Height * scale.Y);
                var destinationRectangle = new Rectangle(x, y, width, height);

                sourceRectangle = ClipSourceRectangle(textureRegion.Bounds, destinationRectangle, clippingRectangle.Value);
                position.X += sourceRectangle.X - textureRegion.Bounds.X;
                position.Y += sourceRectangle.Y - textureRegion.Bounds.Y;

                if (sourceRectangle.Width <= 0 || sourceRectangle.Height <= 0)
                    return;
            }

            spriteBatch.DrawWorld(textureRegion.Texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }


        private static Rectangle ClipSourceRectangle(Rectangle sourceRectangle, Rectangle destinationRectangle, Rectangle clippingRectangle)
        {
            var left = (float)(clippingRectangle.Left - destinationRectangle.Left);
            var right = (float)(destinationRectangle.Right - clippingRectangle.Right);
            var top = (float)(clippingRectangle.Top - destinationRectangle.Top);
            var bottom = (float)(destinationRectangle.Bottom - clippingRectangle.Bottom);
            var x = left > 0 ? left : 0;
            var y = top > 0 ? top : 0;
            var w = (right > 0 ? right : 0) + x;
            var h = (bottom > 0 ? bottom : 0) + y;

            var scaleX = (float)destinationRectangle.Width / sourceRectangle.Width;
            var scaleY = (float)destinationRectangle.Height / sourceRectangle.Height;
            x /= scaleX;
            y /= scaleY;
            w /= scaleX;
            h /= scaleY;

            return new Rectangle((int)(sourceRectangle.X + x), (int)(sourceRectangle.Y + y), (int)(sourceRectangle.Width - w), (int)(sourceRectangle.Height - h));
        }

        public static void DrawStringWorld(this SpriteBatch spriteBatch, BitmapFont bitmapFont, string text, Vector2 position,
            Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float layerDepth, Rectangle? clippingRectangle = null)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            if (effect != SpriteEffects.None)
                throw new NotSupportedException($"{effect} is not currently supported for {nameof(BitmapFont)}");

            var glyphs = bitmapFont.GetGlyphs(text, position);
            foreach (var glyph in glyphs)
            {
                if (glyph.FontRegion == null)
                    continue;
                var characterOrigin = position - glyph.Position + origin;
                spriteBatch.DrawWorld(glyph.FontRegion.TextureRegion, position, color, rotation, characterOrigin, scale, effect, layerDepth, clippingRectangle);
            }
        }
    }
}
