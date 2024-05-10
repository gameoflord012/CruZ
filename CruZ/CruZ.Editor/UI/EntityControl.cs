using System.Collections.Generic;
using System.Linq;

using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.UI;
using CruZ.GameEngine.Input;
using CruZ.GameEngine.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CruZ.Editor.UI
{
    public class EntityControl : UIControl
    {
        private readonly int MinBoundSize = 25;
        private readonly Color DraggingBackgroundColor = Color.Green;

        public EntityControl()
        {
            _initialBackgroundCol = BackgroundColor;
        }

        private IUIRectProvider? ExtractRectUIProvider(TransformEntity entity)
        {
            return entity.GetAllComponents().FirstOrDefault(e => e is IUIRectProvider) as IUIRectProvider;
        }

        private void SetUIRectUIProvider(IUIRectProvider? rectUIProvider)
        {
            if(_currentRectUIProvider != null)
                _currentRectUIProvider.UIRectChanged -= UIRectProvider_ValueChanged;

            _currentRectUIProvider = rectUIProvider;

            if(_currentRectUIProvider != null)
                _currentRectUIProvider.UIRectChanged += UIRectProvider_ValueChanged;
        }

        private void SetRectInfo(UIRect uiRect)
        {
            _worldBound = uiRect.WorldBound;
            _points = uiRect.WorldOrigins;

            if(uiRect.WorldBound == null) return;

            var worldBound = _worldBound!.Value;
            Rect = worldBound.ToScreen(Camera.Current);
        }

        private void SetCenter(Point p)
        {
            Location = new(
                p.X - Width / 2,
                p.Y - Height / 2);
        }

        protected override object? OnStartDragging(IInputInfo input)
        {
            if(_attachedEntity == null || !_canInteract) return null;

            if(Draggable)
            {
                var ePoint = Camera.Current.CoordinateToPoint(_attachedEntity.Transform.Position);
                _dragCenterOffset = new(
                    input.MousePos().X - ePoint.X,
                    input.MousePos().Y - ePoint.Y);
                return this;
            }
            return Draggable ? this : null;
        }

        protected override void OnUpdateDragging(UpdateUIEventArgs info)
        {
            var ePoint = new Point(
                info.InputInfo.MousePos().X - _dragCenterOffset.X,
                info.InputInfo.MousePos().Y - _dragCenterOffset.Y);

            _attachedEntity!.Transform.Position = Camera.Current.PointToCoordinate(ePoint);
        }

        protected override void OnDraw(DrawUIEventArgs args)
        {
            if(_attachedEntity == null || !_canInteract) return;

            base.OnDraw(args);

            foreach(var origin in _points)
            {
                var screen = Camera.Current.CoordinateToPoint(origin);

                args.SpriteBatch.DrawCircle(new(screen.X, screen.Y),
                    EditorConstants.PointSize, 8, Color.Blue);
            }
        }

        protected override void OnUpdate(UpdateUIEventArgs args)
        {
            if(_attachedEntity == null) return;

            if(_worldBound == null)
            {
                Width = MinBoundSize;
                Height = MinBoundSize;
                var center = Camera.Current.CoordinateToPoint(_attachedEntity.Transform.Position);
                SetCenter(center);
            }

            if(!_canInteract) return;

            if(args.InputInfo.IsKeyJustDown(Keys.W))
            {
                Draggable = !Draggable;
            }
        }

        private void UIRectProvider_ValueChanged(UIRect rectInfo)
        {
            SetRectInfo(rectInfo);
        }

        private void Entity_OnRemoveFromWorld(TransformEntity e)
        {
            Parent?.RemoveChild(this);
        }

        private void Entity_ComponentChanged(ComponentCollection comps)
        {
            if(_attachedEntity != null)
                SetUIRectUIProvider(ExtractRectUIProvider(_attachedEntity));
        }

        private bool Draggable
        {
            get => _draggable;
            set
            {
                if(_draggable == value) return;

                _draggable = value;
                BackgroundColor = _draggable ? DraggingBackgroundColor : _initialBackgroundCol;
            }
        }
        public TransformEntity? AttachEntity
        {
            get => _attachedEntity;
            set
            {
                if(_attachedEntity == value) return;

                if(_attachedEntity != null)
                {
                    _attachedEntity.RemovedFromWorld -= Entity_OnRemoveFromWorld;
                    _attachedEntity.ComponentsChanged -= Entity_ComponentChanged;
                }

                _attachedEntity = value;

                if(_attachedEntity != null)
                {
                    _attachedEntity.RemovedFromWorld += Entity_OnRemoveFromWorld;
                    _attachedEntity.ComponentsChanged += Entity_ComponentChanged;

                    var rectUIProvider = ExtractRectUIProvider(_attachedEntity);
                    SetUIRectUIProvider(rectUIProvider);
                }

                IsActive = _attachedEntity != null;
            }
        }
        public bool Active
        {
            get;
            set;
        }

        private Color _initialBackgroundCol;
        private bool _canInteract;
        private TransformEntity? _attachedEntity;
        private IUIRectProvider? _currentRectUIProvider;
        private WorldRectangle? _worldBound;
        private List<Vector2> _points = [];
        private Point _dragCenterOffset;
        private bool _draggable;
    }
}
