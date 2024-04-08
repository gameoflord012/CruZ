using CruZ.Common.ECS;
using CruZ.Editor.Global;
using CruZ.Editor.Service;
using CruZ.Common.UI;

using MonoGame.Extended;

using System;
using System.Collections.Generic;

namespace CruZ.Editor.UI
{
    using System.Linq;

    using CruZ.Framework;
    using CruZ.Framework.GameSystem.ECS;
    using CruZ.Framework.UI;

    using Microsoft.Xna.Framework;

    using SharpDX.Direct3D9;

    public class EntityControl : UIControl, ICanUndo
    {
        static readonly int MIN_BOUND_SIZE = 25;

        public TransformEntity AttachEntity { get => _e; }

        public EntityControl(TransformEntity e)
        {
            _e = e;
            _e.RemovedFromWorld += Entity_OnRemoveFromWorld;
            _e.ComponentsChanged += Entity_ComponentChanged;

            UpdateIHasBoundBox(e.GetAllComponents().FirstOrDefault(e => e is IHasBoundBox) as IHasBoundBox);

            _initialBackgroundCol = BackgroundColor;
            _isSelected = false;
        }

        public void SelectEntity(bool shouldSelect)
        {
            _isSelected = shouldSelect;
            Draggable = false;
        }

        protected override void OnDraw(UIInfo args)
        {
            if(!_isSelected) return;

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
            if (_bounds.IsEmpty)
            {
                Width = MIN_BOUND_SIZE;
                Height = MIN_BOUND_SIZE;
                var center = Camera.Main.CoordinateToPoint(_e.Transform.Position);
                SetCenter(center);
            }

            if (!_isSelected) return;

            if (args.InputInfo.IsKeyJustDown(XNA.Input.Keys.W))
            {
                Draggable = !Draggable;
            }
        }

        private void Entity_BoundingBoxChanged(UIBoundingBox bBox)
        {
            CalcControlBounds(bBox);
        }

        private void Entity_OnRemoveFromWorld(object? sender, EventArgs e)
        {
            if (Parent != null) Parent.RemoveChild(this);
        }

        private void Entity_ComponentChanged(ComponentCollection comps)
        {
            UpdateIHasBoundBox(comps.GetAllComponents().FirstOrDefault(e => e is IHasBoundBox) as IHasBoundBox);
        }

        private void UpdateIHasBoundBox(IHasBoundBox? iHasBoundBox)
        {
            if (_iHasBoundBox != null) _iHasBoundBox.BoundingBoxChanged -= Entity_BoundingBoxChanged;
            _iHasBoundBox = iHasBoundBox;
            if(_iHasBoundBox != null) _iHasBoundBox.BoundingBoxChanged += Entity_BoundingBoxChanged;
        }

        private void CalcControlBounds(UIBoundingBox bBox)
        {
            if(bBox.IsEmpty()) return;

            _bounds = bBox.WorldBounds;
            _points = bBox.WorldOrigins;

            Location = Camera.Main.CoordinateToPoint(new Vector2(_bounds.X, _bounds.Y));
            var LocationBR = Camera.Main.CoordinateToPoint(new Vector2(_bounds.X + _bounds.Width, _bounds.Y + _bounds.Height));

            Width = LocationBR.X - Location.X;
            Height = LocationBR.Y - Location.Y;
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
                    args.MousePos().X - ePoint.X,
                    args.MousePos().Y - ePoint.Y);
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

        bool Draggable
        {
            get => _draggable;
            set
            {
                if (_draggable == value) return;

                _draggable = value;
                BackgroundColor = _draggable ? _draggableBackgroundCol : _initialBackgroundCol;
            }
        }

        TransformEntity _e;
        IHasBoundBox? _iHasBoundBox;
        DRAW.RectangleF _bounds; //World bounds

        DRAW.Point _dragCenterOffset;

        public List<Vector2> _points = [];

        bool _isSelected = false;
        bool _draggable;

        Color _initialBackgroundCol;
        Color _draggableBackgroundCol = Color.Green;
    }
}