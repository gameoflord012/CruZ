using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.GameEngine.GameSystem.Render
{

    /// <summary>
    /// Defines extension methods for the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to render graphical resource types in this library.
    /// </summary>
    public static class SpriteBatchHelper
    {
        public static void Draw(this SpriteBatch spriteBatch, DrawArgs args)
        {
            if(args.Skip) return;
            if(args.Texture == null) throw new ArgumentException("Texture null");
            
            spriteBatch.Draw(
                args.Texture, 
                args.Position, 
                args.SourceRectangle, 
                args.Color, 
                args.Rotation,
                new Vector2(args.NormalizedOrigin.X * args.Texture.Width, args.NormalizedOrigin.Y * args.Texture.Height),
                args.Scale,
                args.SpriteEffect,
                args.LayerDepth);
        }
    }
}
