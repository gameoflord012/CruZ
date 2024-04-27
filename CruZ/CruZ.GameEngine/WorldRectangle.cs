using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace CruZ.GameEngine
{
    /// <summary>
    /// Represent rectangle coordinate in Descartes coordinate 
    /// </summary>
    public struct WorldRectangle
    {
        public float X;
        public float Y;
        public float W;
        public float H;

        public WorldRectangle(float x, float y, float w, float h)
        {
            X = x;
            Y = y;
            W = w;
            H = h;
        }

        public float Top
        {
            get => Y + H;
            set => H = value - Y;
        }

        public float Right
        {
            get => X + W;
            set => W = value - X;
        }
    }
}
