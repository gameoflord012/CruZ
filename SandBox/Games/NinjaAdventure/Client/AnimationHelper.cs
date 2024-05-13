using System;

using CruZ.GameEngine.Utility;
using Microsoft.Xna.Framework;

namespace NinjaAdventure
{
    internal static class AnimationHelper
    {
        public static string GetFacingDirectionString(Vector2 facingDirection, string lastValue = FRONT)
        {
            if (facingDirection.SqrMagnitude() < 0.01f) return lastValue;
            var angle = FunMath.GetAngleBetween(Vector2.UnitY, facingDirection);

            if (FacingDirUnchanged(angle, lastValue)) return lastValue;
            return GetFacingString(angle);
        }

        private static bool FacingDirUnchanged(float angle, string lastValue)
        {
            if (lastValue == BACK)
            {
                return angle.Between(PORTION * -1 - OFFSET, PORTION * 1 + OFFSET);
            }

            else
            if (lastValue == LEFT)
            {
                return angle.Between(PORTION * 1 - OFFSET, PORTION * 3 + OFFSET);
            }

            else
            if (lastValue == RIGHT)
            {
                return angle.Between(PORTION * -3 - OFFSET, PORTION * -1 + OFFSET);
            }

            else
            {
                if (angle < 0) return angle < PORTION * -3 + OFFSET;
                else return angle > PORTION * 3 - OFFSET;
            }
        }

        private static string GetFacingString(float angle)
        {
            if (angle.Between(PORTION * -1, PORTION * 1))
            {
                return BACK;
            }

            if (angle.Between(PORTION * 1, PORTION * 3))
            {
                return LEFT;
            }

            if (angle.Between(PORTION * -3, PORTION * -1))
            {
                return RIGHT;
            }

            return FRONT;
        }

        const string FRONT = "front";
        const string BACK = "back";
        const string LEFT = "left";
        const string RIGHT = "right";

        const float PORTION = MathF.PI / 4;
        const float OFFSET = PORTION / 2;

        private static bool Between(this float a, float b, float c)
        {
            return a >= b && a <= c;
        }
    }
}
