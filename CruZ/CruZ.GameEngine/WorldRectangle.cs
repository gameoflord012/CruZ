using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CruZ.GameEngine.GameSystem;

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

        public RectangleF ToScreen(Camera camera)
        {
            var TL = camera.CoordinateToPoint(new Vector2(X, Y + H));
            var ratio = camera.ScreenToWorldRatio();

            RectangleF screenRect = new();

            screenRect.X = TL.X;
            screenRect.Y = TL.Y;
            screenRect.Width = W * ratio.X;
            screenRect.Height = H * -ratio.Y;

            return screenRect;
        }

        public float Top
        {
            get => Y + H;
        }

        public float Right
        {
            get => X + W;
        }

        public override string ToString()
        {
            return $"<X: {X}, Y: {Y}, W: {W}, H: {H}>";
        }
    }
}
