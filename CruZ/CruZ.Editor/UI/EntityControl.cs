using CruZ.Common.ECS;
using CruZ.Editor.Global;
using CruZ.Editor.Service;
using CruZ.Common.UI;
using CruZ.Common.Utility;

using MonoGame.Extended;

using System;
using System.Collections.Generic;

namespace CruZ.Editor.UI
{
    using CruZ.Common;

    using Microsoft.Xna.Framework;

    public class EntityControl : UIControl, ICanUndo
    {
        static readonly int MIN_BOUND_SIZE = 25;

        public TransformEntity AttachEntity { get => _e; }

        public EntityControl(TransformEntity e)
        {
            _e = e;
            _e.RemoveFromWorldEvent += Entity_OnRemoveFromWorld;
            _e.ComponentChanged += Entity_ComponentChanged;

            if (e.HasComponent(typeof(SpriteComponent)))
            {
                _sp = e.GetComponent<SpriteComponent>();

                _sp.BoundingBoxChanged += Sprite_BoundingBoxChanged;
            }

            _initialBackgroundCol = BackgroundColor;

            Active = false;
        }

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

        protected override void OnDraw(UIInfo args)
        {
            base.OnDraw(args);

            foreach (var origin in _points)
            {
                var screen = Camera.Main.CoordinateToPoint(origin);

                args.SpriteBatch.DrawCircle(new(screen.X, screen.Y),
                    EditorConstants.CENTER_CIRCLE_SIZE, 8, XNA.Color.Blue);
            }
        }

        protected override void OnUpdate(UIInfo args)
        {
            if (args.InputInfo.IsKeyJustDown(XNA.Input.Keys.W))
            {
                _draggableToggle = !_draggableToggle;
            }

            BackgroundColor = _draggableToggle ? _draggableBackgroundCol : _initialBackgroundCol;

            if (_bounds.IsEmpty)
            {
                var center = Camera.Main.CoordinateToPoint(_e.Transform.Position);
                Width = MIN_BOUND_SIZE;
                Height = MIN_BOUND_SIZE;
                SetCenter(center);
                return;
            }
        }

        private void Sprite_BoundingBoxChanged(UIBoundingBox bBox)
        {
            CalcControlBounds(bBox);
        }

        private void Entity_OnRemoveFromWorld(object? sender, EventArgs e)
        {
            if (Parent != null) Parent.RemoveChild(this);
        }

        private void Entity_ComponentChanged(ComponentCollection comps)
        {
            comps.TryGetComponent(ref _sp);
        }

        private void CalcControlBounds(UIBoundingBox bBox)
        {
            if(bBox.IsEmpty()) return;

            _bounds = bBox.Bound;
            _points = bBox.Points;

            Location = Camera.Main.CoordinateToPoint(new Vector2(_bounds.X, _bounds.Y));

            var size = new Size2(
                _bounds.Width * Camera.Main.WorldToScreenScale().X,
                _bounds.Height * Camera.Main.WorldToScreenScale().Y);

            Width = (int)MathF.Max(MIN_BOUND_SIZE, size.Width);
            Height = (int)MathF.Max(MIN_BOUND_SIZE, size.Height);
        }

        private Point GetCenter()
        {
            return new(
                Location.X + (Width + 1) / 2,
                Location.Y + (Height + 1) / 2);
        }

        private void SetCenter(Point p)
        {
            Location = new(
                p.X - Width / 2,
                p.Y - Height / 2);
        }

        #region DRAGGING
        protected override object? OnStartDragging(UIInfo args)
        {
            if (Draggable)
            {
                var ePoint = Camera.Main.CoordinateToPoint(_e.Transform.Position);
                _dragCenterOffset = new(
                    ePoint.X - args.MousePos().X, 
                    ePoint.Y - args.MousePos().Y);
                return this;
            }
            return Draggable ? this : null;
        }

        protected override void OnUpdateDragging(UIInfo info)
        {
            var ePoint = new Point(
                info.MousePos().X - _dragCenterOffset.X,
                info.MousePos().Y - _dragCenterOffset.Y);

            _e.Transform.Position = Camera.Main.PointToCoordinate(ePoint);
        }

        public object CaptureState()
        {
            throw new NotImplementedException();
        }

        public void RestoreState(object state)
        {
            throw new NotImplementedException();
        }

        public bool StatesIdentical(object stateA, object stateB)
        {
            throw new NotImplementedException();
        }
        #endregion

        TransformEntity _e;
        SpriteComponent? _sp;
        DRAW.RectangleF _bounds; //World bounds

        DRAW.Point _dragCenterOffset;

        public List<Vector2> _points = [];

        bool _dragging = false;
        bool _draggableToggle;

        bool Draggable => _draggableToggle;

        XNA.Color _initialBackgroundCol;
        XNA.Color _draggableBackgroundCol = XNA.Color.Green;
    }
}