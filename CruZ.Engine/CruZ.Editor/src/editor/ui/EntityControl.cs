using CruZ.Components;
using CruZ.Editor.Services;
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
    public class EntityControl : UIControl, ICanUndo
    {
        static readonly int MIN_BOUND_SIZE = 25;

        public TransformEntity AttachEntity { get => _e; }

        public EntityControl(TransformEntity e)
        {
            _e = e;
            _e.RemoveFromWorldEvent += Entity_OnRemoveFromWorld;
            _e.ComponentChanged += Entity_ComponentChanged;

            if(e.HasComponent(typeof(SpriteComponent)))
            {
                _sp = e.GetComponent<SpriteComponent>();

                _sp.DrawLoopEnd += Sprite_DrawLoopEnd;
                _sp.DrawBegin += Sprite_DrawBegin;
                _sp.DrawEnd += Sprite_DrawEnd;
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

            if(_hasRenderBound)
            {
                // InternalDraw sprite origin
                foreach (var origin in _origins)
                {
                    var screen = Camera.Main.CoordinateToPoint(origin);

                    args.SpriteBatch.DrawCircle(new(screen.X, screen.Y),
                        EditorVariables.CENTER_CIRCLE_SIZE, 8, XNA.Color.Blue);
                }
            }
        }

        protected override void OnUpdate(UIInfo args)
        {
            if(args.InputInfo.IsKeyJustDown(XNA.Input.Keys.W))
            {
                _draggableToggle = !_draggableToggle;
            }

            BackgroundColor = _draggableToggle ? _draggableBackgroundCol : _initialBackgroundCol;

            if(!_hasRenderBound)
            {
                var center = Camera.Main.CoordinateToPoint(_e.Transform.Position);

                Width = MIN_BOUND_SIZE;
                Height = MIN_BOUND_SIZE;

                SetCenter(center);
            }
        }

        #region Sprites_Events
        private void Sprite_DrawBegin()
        {
            _origins.Clear();
        }

        private void Sprite_DrawEnd(DrawEndEventArgs args)
        {
            CalcControlBounds(args);
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

        private void Entity_ComponentChanged(ComponentCollection comps)
        {
            comps.TryGetComponent(ref _sp);
        }

        private void CalcControlBounds(DrawEndEventArgs args)
        {
            _hasRenderBound = args.HasRenderBounds;
            if(!_hasRenderBound) return;

            _bounds = args.RenderBounds;

            Location = Camera.Main.CoordinateToPoint(new(_bounds.X, _bounds.Y));

            var size = new Size2(
                _bounds.Width * Camera.Main.WorldToScreenScale().X,
                _bounds.Height * Camera.Main.WorldToScreenScale().Y);

            Width = (int)MathF.Max(MIN_BOUND_SIZE, size.Width);
            Height = (int)MathF.Max(MIN_BOUND_SIZE, size.Height);
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
        bool _hasRenderBound;

        DRAW.Point _dragCenterOffset;

        public List<Vector3> _origins = [];

        bool _dragging = false;
        bool _draggableToggle;

        bool Draggable => _draggableToggle;

        XNA.Color _initialBackgroundCol;
        XNA.Color _draggableBackgroundCol = XNA.Color.Green;
    }
}