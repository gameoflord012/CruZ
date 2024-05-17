using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using static Assimp.Metadata;

namespace NinjaAdventure.Ultility
{
    internal static class RandomHelper
    {
        public static Vector2 RandomPosition(this Random random, float radius, Vector2 origin)
        {
            var r = radius * float.Sqrt(random.NextSingle());
            var theta = random.NextSingle() * 2f * MathF.PI;

            Vector2 position = new();

            position.X = origin.X + r * MathF.Cos(theta);
            position.Y = origin.Y + r * MathF.Sin(theta);

            return position;
        }
    }
}
