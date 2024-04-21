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
    public static class SpriteBatchExtensions
    {
        #region Texture Region

        /// <summary>
        /// Draws a <see cref="TextureRegion"/> using the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch">
        /// The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering.
        /// </param>
        /// <param name="region">The <see cref="TextureRegion"/> to render.</param>
        /// <param name="destinationRectangle">
        /// A rectangular bound that defines the destination to render the <see cref="TextureRegion"/> into.
        /// </param>
        /// <param name="color">The color mask to apply when rendering the <see cref="TextureRegion"/>.</param>
        public static void Draw(this SpriteBatch spriteBatch, TextureRegion region, Rectangle destinationRectangle, Color color) =>
            spriteBatch.Draw(region.Texture, destinationRectangle, region.Bounds, color);

        /// <summary>
        /// Draws a <see cref="TextureRegion"/> using the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch">
        /// The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering.
        /// </param>
        /// <param name="region">The <see cref="TextureRegion"/> to render.</param>
        /// <param name="position">The x- and y-coordinate location to render the <see cref="TextureRegion"/> at.</param>
        /// <param name="color">The color mask to apply when rendering the <see cref="TextureRegion"/>.</param>
        public static void Draw(this SpriteBatch spriteBatch, TextureRegion region, Vector2 position, Color color) =>
            spriteBatch.Draw(region.Texture, position, region.Bounds, color);

        /// <summary>
        /// Draws a <see cref="TextureRegion"/> using the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch">
        /// The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering.
        /// </param>
        /// <param name="region">The <see cref="TextureRegion"/> to render.</param>
        /// <param name="position">The x- and y-coordinate location to render the <see cref="TextureRegion"/> at.</param>
        /// <param name="color">The color mask to apply when rendering the <see cref="TextureRegion"/>.</param>
        /// <param name="rotation">
        /// The amount of rotation, in radians, to apply when rendering the <see cref="TextureRegion"/>.
        /// </param>
        /// <param name="origin">
        /// The x- and y-coordinate point of origin to apply when rendering the <see cref="TextureRegion"/>.
        /// </param>
        /// <param name="scale">The amount of scaling to apply when rendering the <see cref="TextureRegion"/>.</param>
        /// <param name="effects">
        /// The <see cref="Microsoft.Xna.Framework.Graphics.SpriteEffects"/> to apply for horizontal and vertical axis 
        /// flipping when rendering the <see cref="TextureRegion"/>.
        /// </param>
        /// <param name="layerDepth">The layer depth to apply when rendering the <see cref="TextureRegion"/>.</param>
        public static void Draw(this SpriteBatch spriteBatch, TextureRegion region, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth) =>
            spriteBatch.Draw(region.Texture, position, region.Bounds, color, rotation, origin, scale, effects, layerDepth);


        /// <summary>
        /// Draws a <see cref="TextureRegion"/> using the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch">
        /// The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering.
        /// </param>
        /// <param name="region">The <see cref="TextureRegion"/> to render.</param>
        /// <param name="position">The x- and y-coordinate location to render the <see cref="TextureRegion"/> at.</param>
        /// <param name="color">The color mask to apply when rendering the <see cref="TextureRegion"/>.</param>
        /// <param name="rotation">
        /// The amount of rotation, in radians, to apply when rendering the <see cref="TextureRegion"/>.
        /// </param>
        /// <param name="origin">
        /// The x- and y-coordinate point of origin to apply when rendering the <see cref="TextureRegion"/>.
        /// </param>
        /// <param name="scale">The amount of scaling to apply when rendering the <see cref="TextureRegion"/>.</param>
        /// <param name="effects">
        /// The <see cref="Microsoft.Xna.Framework.Graphics.SpriteEffects"/> to apply for horizontal and vertical axis 
        /// flipping when rendering the <see cref="TextureRegion"/>.
        /// </param>
        /// <param name="layerDepth">The layer depth to apply when rendering the <see cref="TextureRegion"/>.</param>
        public static void Draw(this SpriteBatch spriteBatch, TextureRegion region, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth) =>
            spriteBatch.Draw(region.Texture, position, region.Bounds, color, rotation, origin, scale, effects, layerDepth);

        /// <summary>
        /// Draws a <see cref="TextureRegion"/> using the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch">
        /// The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering.
        /// </param>
        /// <param name="region">The <see cref="TextureRegion"/> to render.</param>
        /// <param name="destinationRectangle">
        /// A rectangular bound that defines the destination to render the <see cref="TextureRegion"/> into.
        /// </param>
        /// <param name="color">The color mask to apply when rendering the <see cref="TextureRegion"/>.</param>
        /// <param name="rotation">
        /// The amount of rotation, in radians, to apply when rendering the <see cref="TextureRegion"/>.
        /// </param>
        /// <param name="origin">
        /// The x- and y-coordinate point of origin to apply when rendering the <see cref="TextureRegion"/>.
        /// </param>
        /// <param name="effects">
        /// The <see cref="Microsoft.Xna.Framework.Graphics.SpriteEffects"/> to apply for horizontal and vertical axis 
        /// flipping when rendering the <see cref="TextureRegion"/>.
        /// </param>
        /// <param name="layerDepth">The layer depth to apply when rendering the <see cref="TextureRegion"/>.</param>
        public static void Draw(this SpriteBatch spriteBatch, TextureRegion region, Rectangle destinationRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth) =>
            spriteBatch.Draw(region.Texture, destinationRectangle, region.Bounds, color, rotation, origin, effects, layerDepth);

        #endregion Texture Region

        /// <summary>
        /// Draws a <see cref="Sprite"/> using the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch">
        /// The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering the <see cref="Sprite"/>.
        /// </param>
        /// <param name="sprite">The <see cref="Sprite"/> to render.
        /// </param>
        /// <param name="position">The x- and y-coordinate location to render the <see cref="Sprite"/> at.</param>
        public static void Draw(this SpriteBatch spriteBatch, Sprite sprite, Vector2 position) =>
            spriteBatch.Draw(sprite.TextureRegion, position, sprite.Color * sprite.Transparency, sprite.Rotation, sprite.Origin, sprite.Scale, sprite.SpriteEffects, sprite.LayerDepth);
    }
}
