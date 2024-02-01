using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Draw = System.Drawing;

namespace CruZ.UI
{
    public class UIControl
    {
        public virtual void Update(UIArgs args)
        {
        }

        public virtual void Draw(UIArgs args)
        {
            args.SpriteBatch.DrawRectangle(_location, _size, Color.Blue, 5);
            args.SpriteBatch.DrawCircle(_location, 100, 1000, Color.Blue, 5);
        }

        private Vector2 _location;
        private Size2 _size;

        public Draw.Point Location
        {
            get => new((int)_location.X, (int)_location.Y);
            set { _location.X = value.X; _location.Y = value.Y; }
        }

        public int Width    { get => (int)_size.Width;  set => _size.Width = value; }
        public int Height   { get => (int)_size.Height; set => _size.Height = value; }
    }
}