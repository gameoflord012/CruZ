using CruZ.UI;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.Editor.UI
{
    internal class InfoTextWindow : UIControl
    {
        string DisplayText = "Default";

        public InfoTextWindow()
        {
            Location = new(0, 0);
            Width = 300;
            Height = 200;
        }

        protected override void OnDraw(UIInfo args)
        {
            
            //args.SpriteBatch.DrawString()
            base.OnDraw(args);
        }

        SpriteFont _font;
    }
}
