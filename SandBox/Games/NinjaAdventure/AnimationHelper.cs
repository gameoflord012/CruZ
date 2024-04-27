using CruZ.GameEngine.Utility;

using Microsoft.Xna.Framework;

namespace NinjaAdventure
{
    internal static class AnimationHelper
    {
        public static string GetFacingDirectionString(Vector2 facingDirection)
        {
            if(facingDirection.SqrMagnitude() < 0.01f)
                goto RETURN;

            var angle = FunMath.GetAngleBetween(Vector2.UnitY, facingDirection);
            var portion = MathF.PI / 4;

            if (angle.Between(portion * -1, portion * 1))
            {
                return "back";
            }

            if (angle.Between(portion * 1, portion * 3))
            {
                return "right";
            }

            if (angle.Between(portion * -3, portion * -1))
            {
                return "left";
            }

            RETURN:
            return "front";
        }

        private static bool Between(this float a, float b, float c)
        {
            return a >= b && a <= c;
        }
    }
}
