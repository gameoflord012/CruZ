using CruZ.GameEngine.Input;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.GameEngine.GameSystem.UI
{
    public class DrawUIEventArgs
    {
        public DrawUIEventArgs(GameTime gameTime, SpriteBatch spriteBatch)
        {
            GameTime = gameTime;
            SpriteBatch = spriteBatch;
        }

        public GameTime GameTime
        { get; private set; }
        
        public SpriteBatch SpriteBatch
        { get; private set; }
    }
}
