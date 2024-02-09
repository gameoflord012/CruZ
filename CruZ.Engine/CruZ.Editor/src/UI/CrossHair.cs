using MonoGame.Extended;
using System.Drawing;


namespace CruZ.UI
{
    public class CrossHair : UIControl
    {
        protected override void OnDraw(UIInfo args)
        {
            var sp = args.SpriteBatch;

            var vp_Width = GameApplication.Viewport.Width;
            var vp_Height = GameApplication.Viewport.Height;

            var center = new PointF(vp_Width / 2f, vp_Height / 2f);
            sp.DrawLine(center.X, center.Y - 10, center.X, center.Y + 10, XNA.Color.Black);
            sp.DrawLine(center.X - 10, center.Y, center.X + 10, center.Y, XNA.Color.Black);
        }
    }
}