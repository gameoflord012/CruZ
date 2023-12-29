using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ_Engine.Components
{
	interface ISpriteBatchDrawable
	{
        public void Draw(SpriteBatch spriteBatch, Matrix transformMatrix);
    }
}