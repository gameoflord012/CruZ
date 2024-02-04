using CruZ.Components;
using CruZ.Systems;
using CruZ.UI;
using CruZ.Utility;
using MonoGame.Extended;
using System;
using System.Diagnostics;

namespace CruZ.Editor.UI
{
    public class EntityControl : UIControl
    {
        //public event Action<EntityControl>? Selecting;

        public TransformEntity AttachEntity { get => _e; }

        public bool Draggable = false;

        public void SelectEntity(bool shouldSelect)
        {
            if(shouldSelect)
            {
                _showBorder = true;
                Draggable = true;
            }
            else
            {
                _showBorder = false;
                Draggable = false;
            }
        }

        public EntityControl(TransformEntity e)
        {
            _e = e;
            _e.RemoveFromWorldEvent += Entity_OnRemoveFromWorld;
            _sp = e.GetComponent<SpriteComponent>();

            _sp.DrawLoopBegin += Sprite_DrawLoopBegin;
            _sp.DrawLoopEnd += Sprite_DrawLoopEnd;
            _sp.DrawBegin += Sprite_DrawBegin;
            _sp.DrawEnd += Sprite_DrawEnd;
        }

        protected override void OnUpdate(UIArgs args)
        {
            base.OnUpdate(args);

            if(Draggable && _dragging)
            {
                var ePoint = args.MousePos().Add(_dragCenterOffset);
                _e.Transform.Position = Camera.Main.PointToCoordinate(ePoint);

            }
        }

        protected override void OnDraw(UIArgs args)
        {
            if (_showBorder)
                base.OnDraw(args);
        }

        protected override void OnMouseStateChange(UIArgs args)
        {
            if(Draggable)
            {
                if(args.InputInfo.IsMouseDown(MouseKey.Left) && !_dragging)
                {
                    _dragging = true;

                    var  ePoint = Camera.Main.CoordinateToPoint(_e.Transform.Position);
                    _dragCenterOffset = ePoint.Minus(args.MousePos());
                }
                else if(args.InputInfo.IsMouseUp(MouseKey.Left))
                {
                    _dragging = false;
                }
            }
        }

        private void Sprite_DrawBegin()
        {
            _bounds.X = _e.Position.X;
            _bounds.Y = _e.Position.Y;

            _bounds.Width = 0;
            _bounds.Height = 0;
        }

        private void Sprite_DrawEnd()
        {
            CalcBounds();
        }

        private void Sprite_DrawLoopBegin(object? sender, DrawBeginEventArgs args)
        {
            _args = args;
        }

        private void Sprite_DrawLoopEnd(object? sender, DrawEndEventArgs e)
        {
            var rect = _args.BoundRect();
            _bounds.X = MathF.Min(_bounds.X, rect.X);
            _bounds.Y = MathF.Min(_bounds.Y, rect.Y);
            _bounds.Width = _bounds.Right < rect.Right ? rect.Right - _bounds.X : _bounds.Width;
            _bounds.Height = _bounds.Bottom < rect.Bottom ? rect.Bottom - _bounds.Y : _bounds.Height;
        }

        private void Entity_OnRemoveFromWorld(object? sender, EventArgs e)
        {
            if (Parent != null) Parent.RemoveChild(this);
        }

        private void CalcBounds()
        {
            Location = Camera.Main.CoordinateToPoint(new(_bounds.X, _bounds.Y));

            var size = new Size2
                (_bounds.Width * Camera.Main.WorldToScreenScale().X,
                _bounds.Height * Camera.Main.WorldToScreenScale().Y);

            Width = (int)size.Width;
            Height = (int)size.Height;
        }

        private DRAW.Point GetCenter()
        {
            return new(
                Location.X + (Width + 1) / 2,
                Location.Y + (Height + 1) / 2);
        }

        private void SetCenter(DRAW.Point p)
        {
            Location = new(
                p.X - Width / 2,
                p.Y - Height / 2);
        }

        TransformEntity _e;
        SpriteComponent _sp;
        DrawBeginEventArgs _args;
        RectangleF _bounds; //World bounds

        DRAW.Point _dragCenterOffset;

        bool _dragging = false;
        bool _showBorder = false;
    }
}