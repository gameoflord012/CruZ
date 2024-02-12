using CruZ.Components;
using CruZ.Systems;
using CruZ.UI;
using CruZ.Utility;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

namespace CruZ.Editor.UI
{
    public class EntityControl : UIControl
    {
        //public event Action<EntityControl>? Selecting;

        public TransformEntity AttachEntity { get => _e; }

        public void SelectEntity(bool shouldSelect)
        {
            if (shouldSelect)
            {
                Active = true;
            }
            else
            {
                Active = false;
            }

            _draggableToggle = false;
        }

        public EntityControl(TransformEntity e)
        {
            _e = e;
            _e.RemoveFromWorldEvent += Entity_OnRemoveFromWorld;
            _sp = e.GetComponent<SpriteComponent>();

            _sp.DrawLoopEnd += Sprite_DrawLoopEnd;
            _sp.DrawBegin += Sprite_DrawBegin;
            _sp.DrawEnd += Sprite_DrawEnd;

            _initialBackgroundCol = BackgroundColor;

            Active = false;
        }

        protected override void OnDraw(UIInfo args)
        {
            base.OnDraw(args);

            foreach (var origin in _origins)
            {
                var screen = Camera.Main.CoordinateToPoint(origin);
                args.SpriteBatch.DrawCircle(new(screen.X, screen.Y),
                    EditorVariables.CENTER_CIRCLE_SIZE, 8, XNA.Color.Blue);
            }
        }

        protected override void OnUpdate(UIInfo args)
        {
            if(args.InputInfo.IsKeyJustDown(XNA.Input.Keys.W))
            {
                _draggableToggle = !_draggableToggle;
            }

            BackgroundColor = _draggableToggle ? _draggableBackgroundCol : _initialBackgroundCol;
        }

        #region Sprites_Events
        private void Sprite_DrawBegin()
        {
            _boundsHasValue = false;

            _origins.Clear();
        }

        private void Sprite_DrawEnd(DrawEndEventArgs args)
        {
            _bounds = args.RenderBounds;
            CalcBounds();
        }

        private void Sprite_DrawLoopEnd(object? sender, DrawLoopEndEventArgs args)
        {
            _origins.Add(args.BeginArgs.GetWorldOrigin());

        }
        #endregion

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

        #region DRAGGING
        protected override object? OnStartDragging(UIInfo args)
        {
            if(Draggable)
            {
                var ePoint = Camera.Main.CoordinateToPoint(_e.Transform.Position);
                _dragCenterOffset = ePoint.Minus(args.MousePos());
                return this;
            }
            return Draggable ? this : null;
        }

        protected override void OnUpdateDragging(UIInfo info)
        {
            var ePoint = info.MousePos().Add(_dragCenterOffset);
            _e.Transform.Position = Camera.Main.PointToCoordinate(ePoint);
        }
        #endregion

        TransformEntity _e;
        SpriteComponent _sp;
        DRAW.RectangleF _bounds; //World bounds

        DRAW.Point _dragCenterOffset;

        public List<Vector3> _origins = [];

        bool _dragging = false;
        bool _boundsHasValue = false;
        bool _draggableToggle;

        bool Draggable => _draggableToggle;

        XNA.Color _initialBackgroundCol;
        XNA.Color _draggableBackgroundCol = XNA.Color.Green;
    }
}