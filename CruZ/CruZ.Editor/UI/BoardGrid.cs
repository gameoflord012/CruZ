using Microsoft.Xna.Framework.Graphics;

using System.Drawing;

using Color = Microsoft.Xna.Framework.Color;

namespace CruZ.Editor.UI;

using System.Numerics;

using CruZ.Framework;
using CruZ.Framework.GameSystem.UI;
using CruZ.Framework.Utility;

public class BoardGrid : UIControl
{
    protected override void OnDraw(UIInfo args)
    {
        var sp = args.SpriteBatch;

        DrawAxis(args.SpriteBatch);

        var vp_Width = GameApplication.GetGraphicsDevice().Viewport.Width;
        var vp_Height = GameApplication.GetGraphicsDevice().Viewport.Height;

        // draw cross-hair
        var center = new PointF(vp_Width / 2f, vp_Height / 2f);
        sp.DrawLine(center.X, center.Y - 10, center.X, center.Y + 10, Color.Black);
        sp.DrawLine(center.X - 10, center.Y, center.X + 10, center.Y, Color.Black);
    }

    private void DrawAxis(SpriteBatch spriteBatch)
    {
        const int MAX_LINE_IN_SCREEN = 25;

        float minLineDistance = Camera.Main.ViewPortWidth / MAX_LINE_IN_SCREEN;
        int lineWorldDis = 1;

        // x2 if lineWorldDis not excess minLineDistance
        while (lineWorldDis * Camera.Main.ScreenToWorldRatio().X < minLineDistance)
            lineWorldDis *= 2;

        var col = lineWorldDis == 1 ?
            EditorConstants.UNIT_BOARD_COLOR :
            EditorConstants.BOARD_COLOR;

        DrawBoard(FunMath.RoundInt(lineWorldDis), col);

        void DrawBoard(int lineDis, Color col)
        {
            var center = Camera.Main.CameraOffset;

            var x_distance = Camera.Main.ViewPortWidth / Camera.Main.ScreenToWorldRatio().X;
            var y_distance = Camera.Main.ViewPortHeight / Camera.Main.ScreenToWorldRatio().Y;

            var min_x = center.X - x_distance;
            var max_x = center.X + x_distance;

            var min_y = center.Y - y_distance;
            var max_y = center.Y + y_distance;

            for (float x = (int)min_x / lineDis * lineDis; x < max_x; x += lineDis)
            {
                var p1 = Camera.Main.CoordinateToPoint(new Vector2(x, min_y));
                var p2 = Camera.Main.CoordinateToPoint(new Vector2(x, max_y));

                spriteBatch.DrawLine(p1.X, p1.Y, p2.X, p2.Y, col);
            }

            for (float y = (int)min_y / lineDis * lineDis; y < max_y; y += lineDis)
            {
                var p1 = Camera.Main.CoordinateToPoint(new(min_x, y));
                var p2 = Camera.Main.CoordinateToPoint(new(max_x, y));

                spriteBatch.DrawLine(p1.X, p1.Y, p2.X, p2.Y, col);
            }
        }
    }
}