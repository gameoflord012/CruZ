using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;

namespace CruZ.GameEngine.GameSystem.Render
{
    public static class SpriteBatchHelper
    {
        public static void Draw(this SpriteBatch spriteBatch, DrawArgs args)
        {
            if (args.Skip) return;

            spriteBatch.Draw(
                texture: args.Texture,
                position: args.Position,

                sourceRectangle: args.SourceRectangle,

                color: args.Color,
                rotation: 0,

                origin: new(args.NormalizedOrigin.X * args.SourceRectangle.Width,
                            args.NormalizedOrigin.Y * args.SourceRectangle.Height),

                scale: args.Scale,

                effects: args.Flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                layerDepth: args.LayerDepth);
        }
    }
}
