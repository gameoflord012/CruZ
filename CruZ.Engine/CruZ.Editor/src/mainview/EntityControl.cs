//using CruZ.Components;
//using CruZ.Systems;
//using System.Drawing;
//using System.Windows.Forms;

//namespace CruZ.Editor.Controls
//{
//    class EntityControl : UserControl
//    {
//        public EntityControl(TransformEntity e)
//        {
//            _e = e;
//            _sp = e.GetComponent<SpriteComponent>();

//            if (_e.HasComponent(typeof(TileComponent))) return;

//            _sp.OnDrawBegin += Sprite_BeginDraw;
//            _sp.OnDrawEnd += Sprite_EndDraw;
//        }

//        protected override void OnPaintBackground(PaintEventArgs e)
//        {
//            e.Graphics.FillRectangle(Brushes.LimeGreen, e.ClipRectangle);

//            using Pen p = new Pen(Color.Red);
//            e.Graphics.DrawLine(p, 0, 10, 10000, 0);
//        }

//        private void Sprite_BeginDraw(object? sender, DrawBeginEventArgs e)
//        {
//            _args = e;
//        }

//        private void Sprite_EndDraw(object? sender, DrawEndEventArgs e)
//        {
//            //var coord = _args.Position;
//            //var rect = _args.SourceRectangle;
//            //var point = Camera.Main.CoordinateToPoint(coord);

//            //Width = (int)(rect.Width * Camera.Main.WorldToScreenScale().X);
//            //Height = (int)(rect.Height * Camera.Main.WorldToScreenScale().Y);
//            //Location = new(point.X - Width / 2, point.Y - Height / 2);

//            //Invalidate();
//        }

//        TransformEntity _e;
//        SpriteComponent _sp;
//        DrawBeginEventArgs _args;
//    }
//}
