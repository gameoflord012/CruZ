using CruZ.Systems;
using CruZ.Utility;
using System.Drawing;
using MonoGame.Extended;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.Editor.Controls
{
    public partial class EditorApplication
    {
        private void DrawAxis(SpriteBatch spriteBatch)
        {
            const int MAX_X_LINE = 25;

            spriteBatch.Begin();

            float maxLineDistance = Camera.Main.ViewPortWidth / MAX_X_LINE;
            int lineDis = 1;

            while (
                (float)lineDis * 2f *
                Camera.Main.WorldToScreenScale().X < maxLineDistance)
            {
                lineDis *= 2;
            }

            var col = lineDis == 1 ?
                EditorVariables.UNIT_BOARD_COLOR :
                XNA.Color.DarkGray;

            DrawBoard(FunMath.RoundInt(lineDis), col);

            spriteBatch.End();

            void DrawBoard(int lineDis, XNA.Color col)
            {
                var center = Camera.Main.Position;

                var x_distance = Camera.Main.VirtualWidth;
                var y_distance = Camera.Main.VirtualHeight;

                var min_x = center.X - x_distance;
                var max_x = center.X + x_distance;

                var min_y = center.Y - y_distance;
                var max_y = center.Y + y_distance;

                for (float x = (int)min_x / lineDis * lineDis; x < max_x; x += lineDis)
                {
                    var p1 = Camera.Main.CoordinateToPoint(new Vector3(x, min_y));
                    var p2 = Camera.Main.CoordinateToPoint(new Vector3(x, max_y));

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
}
