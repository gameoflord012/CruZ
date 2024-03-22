using System;
using System.Numerics;

namespace CruZ.Framework.Utility
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

        public static float SqrMagnitude(this XNA.Vector2 v)
        {
            return v.X * v.X + v.Y * v.Y;
        }

        public static int RoundToInt(this float f)
        {
            return (int)(f + 0.5f);
        }

        public static float[,] GenPerlinNoise(int w, int h)
        {
            FastNoiseLite noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);

            var noiseData = new float[w, h];
            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                    noiseData[i, j] = (noise.GetNoise(i, j) + 1f) / 2f;

            return noiseData;
        }
    }
}