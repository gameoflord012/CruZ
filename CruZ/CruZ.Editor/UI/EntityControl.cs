using CruZ.Editor.Service;

using System;
using System.Collections.Generic;

using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using RectangleF = System.Drawing.RectangleF;
using CruZ.GameEngine.GameSystem.UI;
using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.Utility;
using CruZ.GameEngine;

namespace CruZ.Editor.UI
{
    public class EntityControl : UIControl, ICanUndo
    {
        static readonly int MIN_BOUND_SIZE = 25;

        public EntityControl()
        {
            _initialBackgroundCol = BackgroundColor;
        }

        public TransformEntity? AttachEntity
        {
            get => _attachedEntity;
            set
            {
                if (_attachedEntity == value) return;

                if (_attachedEntity != null)
                {
                    _attachedEntity.BecameDrity -= Entity_OnRemoveFromWorld;
                    _attachedEntity.ComponentsChanged -= Entity_ComponentChanged;
                }

                _attachedEntity = value;

                if (_attachedEntity != null)
                {
                    _attachedEntity.BecameDrity += Entity_OnRemoveFromWorld;
                    _attachedEntity.ComponentsChanged += Entity_ComponentChanged;

                    var rectUIProvider = ExtractRectUIProvider(_attachedEntity);
                    SetUIRectUIProvider(rectUIProvider);
                }

                IsActive = _attachedEntity != null;
            }
        }

        TransformEntity? _attachedEntity;

        public bool CanInteract
        {
            get => _canInteract;
            set
            {
                if(_canInteract == value) return;
                Draggable = false;
                _canInteract = value;
            }
        }

        bool _canInteract = false;

        private void Entity_ComponentChanged(ComponentCollection comps)
        {
            if (_attachedEntity != null)
                SetUIRectUIProvider(ExtractRectUIProvider(_attachedEntity));
        }

        private IRectUIProvider? ExtractRectUIProvider(TransformEntity entity)
        {
            return entity.GetAllComponents().FirstOrDefault(e => e is IRectUIProvider) as IRectUIProvider;
        }

        private void SetUIRectUIProvider(IRectUIProvider? rectUIProvider)
        {
            if (_currentRectUIProvider != null) 
                _currentRectUIProvider.UIRectChanged -= RectUIProvider_ValueChanged;

            _currentRectUIProvider = rectUIProvider;

            if (_currentRectUIProvider != null)
                _currentRectUIProvider.UIRectChanged += RectUIProvider_ValueChanged;
        }

        protected override void OnDraw(UIInfo args)
        {
            if (_attachedEntity == null || !_canInteract) return;

            base.OnDraw(args);

            foreach (var origin in _points)
            {
                var screen = Camera.Main.CoordinateToPoint(origin);

                args.SpriteBatch.DrawCircle(new(screen.X, screen.Y),
                    EditorConstants.CENTER_CIRCLE_SIZE, 8, Color.Blue);
            }
        }

        protected override void OnUpdate(UIInfo args)
        {
            if (_attachedEntity == null) return;

            if (_worldBound == null)
            {
                Width = MIN_BOUND_SIZE;
                Height = MIN_BOUND_SIZE;
                var center = Camera.Main.CoordinateToPoint(_attachedEntity.Transform.Position);
                SetCenter(center);
            }

            if (!_canInteract) return;

            if (args.InputInfo.IsKeyJustDown(Keys.W))
            {
                Draggable = !Draggable;
            }
        }

        private void RectUIProvider_ValueChanged(RectUIInfo rectInfo)
        {
            SetRectInfo(rectInfo);
        }

        private void Entity_OnRemoveFromWorld(TransformEntity e)
        {
            if (Parent != null) Parent.RemoveChild(this);
        }

        private void SetRectInfo(RectUIInfo uiRect)
        {
            _worldBound = uiRect.WorldBound;
            _points = uiRect.WorldOrigins;

            if (uiRect.WorldBound == null) return;

            var worldBound = _worldBound!.Value;
            Location = Camera.Main.CoordinateToPoint(new Vector2(worldBound.X, worldBound.Y));
            Width = (worldBound.W * Camera.Main.ScreenToWorldRatio().X).RoundToInt();
            Height = (worldBound.H * Camera.Main.ScreenToWorldRatio().Y).RoundToInt();
        }

        private void SetCenter(Point p)
        {
            Location = new(
                p.X - Width / 2,
                p.Y - Height / 2);
        }

        IRectUIProvider? _currentRectUIProvider;
        WorldRectangle? _worldBound; //ECSWorld bounds
        List<Vector2> _points = [];

        protected override object? OnStartDragging(UIInfo args)
        {
            if (_attachedEntity == null || !_canInteract) return null;

            if (Draggable)
            {
                var ePoint = Camera.Main.CoordinateToPoint(_attachedEntity.Transform.Position);
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

            _attachedEntity!.Transform.Position = Camera.Main.PointToCoordinate(ePoint);
        }

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

        Point _dragCenterOffset;
        bool _draggable;

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

        Color _initialBackgroundCol;
        Color _draggableBackgroundCol = Color.Green;
    }
}