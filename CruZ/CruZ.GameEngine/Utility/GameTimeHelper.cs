using Microsoft.Xna.Framework;

namespace CruZ.GameEngine.Utility
{
    public static class GameTimeHelper
    {
        public static float TotalGameTime(this GameTime gameTime)
        {
            return (float)gameTime.TotalGameTime.TotalSeconds;
        }

        public static float DeltaTime(this GameTime gameTime)
        {
            return (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}