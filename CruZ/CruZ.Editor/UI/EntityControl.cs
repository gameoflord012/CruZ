using System.Collections.Generic;
using System.Linq;

using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.UI;
using CruZ.GameEngine.Utility;

using Microsoft.Xna.Framework;

namespace CruZ.Editor.UI
{
    public class EntityControl : UIControl, IPoolObject
    {
        private readonly int MinBoundSize = 25;

        public EntityControl() : base()
        {
            IsActive = false;
        }

        public void Reset(TransformEntity entity)
        {
            AttachedEntity = entity;
            IsActive = false;
        }

        private IUIRectProvider? ExtractRectUIProvider(TransformEntity entity)
        {
            return entity.GetAllComponents().FirstOrDefault(e => e is IUIRectProvider) as IUIRectProvider;
        }

        private void SetUIRectUIProvider(IUIRectProvider? rectUIProvider)
        {
            uiRectProvider = rectUIProvider;
        }

        private void ApplyRectInfo(UIRect uiRect)
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

        protected override void OnDraw(DrawUIEventArgs args)
        {
            if(_attachedEntity == null) return;

            if(uiRectProvider != null)
            {
                ApplyRectInfo(uiRectProvider.UIRect);
            }

            base.OnDraw(args);

            foreach(var origin in _points)
            {
                var screen = Camera.Current.CoordinateToPoint(origin);

                args.SpriteBatch.DrawCircle(
                    new(screen.X, screen.Y),
                    EditorConstants.PointSize, 8, Color.Blue);
            }
        }

        protected override void OnUpdate(UpdateUIEventArgs args)
        {
            if(_attachedEntity == null) return;

            if(_worldBound.HasValue)
            {
                Width = MinBoundSize;
                Height = MinBoundSize;
                var center = Camera.Current.CoordinateToPoint(_attachedEntity.Transform.Position);
                SetCenter(center);
            }
        }

        private void Entity_Destroying(TransformEntity e)
        {
            ReturnToPool();
        }

        private void Entity_ComponentChanged(ComponentCollection comps)
        {
            if(_attachedEntity != null)
            {
                SetUIRectUIProvider(ExtractRectUIProvider(_attachedEntity));
            }
        }

        private void ReturnToPool()
        {
            ((IPoolObject)this).Pool.ReturnPoolObject(this);
        }

        void IPoolObject.OnDisabled()
        {
            IsActive = false;
            AttachedEntity = null;
        }

        public TransformEntity? AttachedEntity
        {
            get => _attachedEntity;
            set
            {
                if(_attachedEntity == value) return;

                if(_attachedEntity != null)
                {
                    _attachedEntity.Destroying -= Entity_Destroying;
                    _attachedEntity.ComponentsChanged -= Entity_ComponentChanged;
                }

                _attachedEntity = value;

                if(_attachedEntity != null)
                {
                    _attachedEntity.Destroying += Entity_Destroying;
                    _attachedEntity.ComponentsChanged += Entity_ComponentChanged;

                    var rectUIProvider = ExtractRectUIProvider(_attachedEntity);
                    SetUIRectUIProvider(rectUIProvider);
                }
            }
        }

        Pool IPoolObject.Pool
        {
            get;
            set;
        }

        private TransformEntity? _attachedEntity;
        private IUIRectProvider? uiRectProvider;
        private WorldRectangle? _worldBound;
        private List<Vector2> _points = [];
    }
}
