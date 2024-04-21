//// Copyright (c) Christopher Whitley. All rights reserved.
//// Licensed under the MIT license.
//// See LICENSE file in the project root for full license information.

//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace CruZ.GameEngine.GameSystem.Render;

///// <summary>
/////     Defines a named rectangular region that represents the location and extents of a region within a source texture.
///// </summary>
//public class TextureRegion
//{
//    /// <summary>
//    /// Gets the name assigned to this <see cref="TextureRegion"/>.
//    /// </summary>
//    public string Name { get; }

//    /// <summary>
//    /// Gets the source texture used by this <see cref="TextureRegion"/>.
//    /// </summary>
//    public Texture2D Texture { get; }

//    /// <summary>
//    /// Gets the rectangular bounds that define the location and width and height extents, in pixels, of the region
//    /// within the source texture that is represented by this <see cref="TextureRegion"/>.
//    /// </summary>
//    public Rectangle Bounds { get; }
//    /// <summary>
//    /// Initializes a new instance of the <see cref="TextureRegion"/> class.
//    /// </summary>
//    /// <param name="name">The name to assign the <see cref="TextureRegion"/>.</param>
//    /// <param name="texture">The source texture image this region is from.</param>
//    /// <param name="bounds">The rectangular bounds of this region within the source texture.</param>
//    public TextureRegion(string name, Texture2D texture, Rectangle bounds) =>
//        (Name, Texture, Bounds) = (name, texture, bounds);

//    /// <summary>
//    /// Draws this <see cref="TextureRegion"/> instance using the 
//    /// <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> provided.
//    /// </summary>
//    /// <param name="spriteBatch">
//    /// The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering.
//    /// </param>
//    /// <param name="destinationRectangle">
//    /// A rectangular bound that defines the destination to render this <see cref="TextureRegion"/> into.
//    /// </param>
//    /// <param name="color">The color mask to apply when rendering this <see cref="TextureRegion"/>.</param>
//    public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle, Color color) =>
//        spriteBatch.Draw(this, destinationRectangle, color);

//    /// <summary>
//    /// Draws this <see cref="TextureRegion"/> instance using the 
//    /// <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> provided.
//    /// </summary>
//    /// <param name="spriteBatch">
//    /// The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering.
//    /// </param>
//    /// <param name="position">The x- and y-coordinate location to render this <see cref="TextureRegion"/> at.</param>
//    /// <param name="color">The color mask to apply when rendering this <see cref="TextureRegion"/>.</param>
//    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color) =>
//        spriteBatch.Draw(this, position, color);

//    /// <summary>
//    /// Draws this <see cref="TextureRegion"/> instance using the 
//    /// <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> provided.
//    /// </summary>
//    /// <param name="spriteBatch">
//    /// The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering.
//    /// </param>
//    /// <param name="position">The x- and y-coordinate location to render this <see cref="TextureRegion"/> at.</param>
//    /// <param name="color">The color mask to apply when rendering this <see cref="TextureRegion"/>.</param>
//    /// <param name="rotation">
//    /// The amount of rotation, in radians, to apply when rendering this <see cref="TextureRegion"/>.
//    /// </param>
//    /// <param name="origin">
//    /// The x- and y-coordinate point of origin to apply when rendering this <see cref="TextureRegion"/>.
//    /// </param>
//    /// <param name="scale">The amount of scaling to apply when rendering this <see cref="TextureRegion"/>.</param>
//    /// <param name="effects">
//    /// The <see cref="Microsoft.Xna.Framework.Graphics.SpriteEffects"/> to apply for horizontal and vertical axis 
//    /// flipping when rendering this <see cref="TextureRegion"/>.
//    /// </param>
//    /// <param name="layerDepth">The layer depth to apply when rendering this <see cref="TextureRegion"/>.</param>
//    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth) =>
//        spriteBatch.Draw(this, position, color, rotation, origin, scale, effects, layerDepth);

//    /// <summary>
//    /// Draws this <see cref="TextureRegion"/> instance using the 
//    /// <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> provided.
//    /// </summary>
//    /// <param name="spriteBatch">
//    /// The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering.
//    /// </param>
//    /// <param name="position">The x- and y-coordinate location to render this <see cref="TextureRegion"/> at.</param>
//    /// <param name="color">The color mask to apply when rendering this <see cref="TextureRegion"/>.</param>
//    /// <param name="rotation">
//    /// The amount of rotation, in radians, to apply when rendering this <see cref="TextureRegion"/>.
//    /// </param>
//    /// <param name="origin">
//    /// The x- and y-coordinate point of origin to apply when rendering this <see cref="TextureRegion"/>.
//    /// </param>
//    /// <param name="scale">The amount of scaling to apply when rendering this <see cref="TextureRegion"/>.</param>
//    /// <param name="effects">
//    /// The <see cref="Microsoft.Xna.Framework.Graphics.SpriteEffects"/> to apply for horizontal and vertical axis 
//    /// flipping when rendering this <see cref="TextureRegion"/>.
//    /// </param>
//    /// <param name="layerDepth">The layer depth to apply when rendering this <see cref="TextureRegion"/>.</param>
//    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth) =>
//        spriteBatch.Draw(this, position, color, rotation, origin, scale, effects, layerDepth);

//    /// <summary>
//    /// Draws this <see cref="TextureRegion"/> instance using the 
//    /// <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> provided.
//    /// </summary>
//    /// <param name="spriteBatch">
//    /// The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering.
//    /// </param>
//    /// <param name="destinationRectangle">
//    /// A rectangular bound that defines the destination to render this <see cref="TextureRegion"/> into.
//    /// </param>
//    /// <param name="color">The color mask to apply when rendering this <see cref="TextureRegion"/>.</param>
//    /// <param name="rotation">
//    /// The amount of rotation, in radians, to apply when rendering this <see cref="TextureRegion"/>.
//    /// </param>
//    /// <param name="origin">
//    /// The x- and y-coordinate point of origin to apply when rendering this <see cref="TextureRegion"/>.
//    /// </param>
//    /// <param name="effects">
//    /// The <see cref="Microsoft.Xna.Framework.Graphics.SpriteEffects"/> to apply for horizontal and vertical axis 
//    /// flipping when rendering this <see cref="TextureRegion"/>.
//    /// </param>
//    /// <param name="layerDepth">The layer depth to apply when rendering this <see cref="TextureRegion"/>.</param>
//    public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth) =>
//        spriteBatch.Draw(this, destinationRectangle, color, rotation, origin, effects, layerDepth);
//}
