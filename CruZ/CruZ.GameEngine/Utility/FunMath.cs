using System;

using Microsoft.Xna.Framework;

namespace CruZ.GameEngine.Utility
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

        public static Point Minus(this Point p1, Point p2)
        {
            return new(p1.X - p2.X, p1.Y - p2.Y);
        }

        public static Point Add(this Point p1, Point p2)
        {
            return new(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static float SqrMagnitude(this Vector2 v)
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

        public static float GetRotation(Vector2 v1, Vector2 v2)
        {
            v1.Normalize();
            v2.Normalize();

            var angle = MathF.Acos(Vector2.Dot(v1, v2));
            var cross = v1.X * v2.Y - v1.Y * v2.X;

            return cross < 0 ? angle : cross > 0 ? -angle : 0;
        }
    }
}