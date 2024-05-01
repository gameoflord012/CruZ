using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.BitmapFonts;

namespace CruZ.GameEngine.GameSystem.Render
{
    public class StringDrawRequest : DrawRequestBase
    {
        private BitmapFont _font;
        private string _text;
        private Vector2 _position;

        public StringDrawRequest(BitmapFont font, string text, Vector2 position)
        {
            _font = font;
            _text = text;
            _position = position;
        }

        public override void DoRequest(SpriteBatch spriteBatch)
        {
            Vector2 origin = _font.MeasureString(_text) / 2f;

            spriteBatch.DrawStringWorld(
                _font, 
                _text, 
                _position, 
                Color.White, 
                rotation: 0, 
                origin: origin, 
                scale: Vector2.One, 
                SpriteEffects.None, 
                layerDepth: 0, 
                clippingRectangle: null);
        }
    }
}
