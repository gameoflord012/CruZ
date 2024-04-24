using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace CruZ.GameEngine.GameSystem.Render
{
    public class FetchingDrawRequestsEventArgs
    {
        public FetchingDrawRequestsEventArgs(DrawArgs drawArgs, List<DrawArgs> drawRequests, Matrix viewProjectionMat)
        {
            DefaultDrawArgs = drawArgs;
            DrawRequests = drawRequests;
            ViewProjectionMat = viewProjectionMat;
        }

        public bool IsDrawRequestOutOfScreen(DrawArgs args)
        {
            RectangleF bounds = args.GetWorldBounds();
            
            var TL = new Vector4(bounds.Left, bounds.Top, 0, 1);
            var BR = new Vector4(bounds.Right, bounds.Bottom, 0, 1);

            TL = Vector4.Transform(TL, ViewProjectionMat);
            BR = Vector4.Transform(BR, ViewProjectionMat);

            return TL.X > 1 || BR.X < -1 || TL.Y < -1 || BR.Y > 1;
        }

        public DrawArgs DefaultDrawArgs { get; }
        public List<DrawArgs> DrawRequests { get; }
        public Matrix ViewProjectionMat { get; }
    }
}
