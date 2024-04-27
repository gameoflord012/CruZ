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
            WorldRectangle worldBounds = args.GetWorldBounds();
            
            var min = new Vector4(worldBounds.X, worldBounds.Y, 0, 1);
            var max = new Vector4(worldBounds.Right, worldBounds.Top, 0, 1);

            var matrix = Camera.Main.ViewProjectionMatrix();

            var minNDC = Vector4.Transform(min, matrix);
            var maxNDC = Vector4.Transform(max, matrix);

            return maxNDC.X < -1 || maxNDC.Y < -1 || minNDC.X > 1 || minNDC.Y > 1;
        }

        public DrawArgs DefaultDrawArgs { get; }
        public List<DrawArgs> DrawRequests { get; }
        public Matrix ViewProjectionMat { get; }
    }
}
