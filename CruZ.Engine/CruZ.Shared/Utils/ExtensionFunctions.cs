using Microsoft.Xna.Framework;

namespace CruZ.Utility
{
    public static class ExtensionFunctions
    {
        public static float TotalSeconds(this GameTime gameTime)
        {
            return (float)gameTime.TotalGameTime.TotalSeconds;
        }
    }
}