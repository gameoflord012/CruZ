using System;
using System.Numerics;

namespace CruZ.Common.Utility
{
    public static class FunMath
    {
        public static int RoundInt(float f)
        {
            return (int)(f + 0.5f);
        }

        public static int CeilInt(float f)
        {
            return RoundInt(MathF.Ceiling(f));
        }

        public static DRAW.Point Minus(this DRAW.Point p1, DRAW.Point p2)
        {
            return new(p1.X - p2.X, p1.Y - p2.Y);
        }

        public static DRAW.Point Add(this DRAW.Point p1, DRAW.Point p2)
        {
            return new(p1.X + p2.X, p1.Y + p2.Y);
        }
    }
}