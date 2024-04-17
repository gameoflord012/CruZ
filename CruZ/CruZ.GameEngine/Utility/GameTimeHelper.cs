using Microsoft.Xna.Framework;

namespace CruZ.GameEngine.Utility
{
    public static class GameTimeHelper
    {
        public static float TotalSeconds(this GameTime gameTime)
        {
            return (float)gameTime.TotalGameTime.TotalSeconds;
        }
    }
}