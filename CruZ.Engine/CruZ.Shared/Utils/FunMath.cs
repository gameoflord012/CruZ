using System;
using System.Numerics;

namespace CruZ.Utility
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
    }
}