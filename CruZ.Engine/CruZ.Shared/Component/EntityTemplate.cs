using MonoGame.Extended.Entities;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace CruZ.Components
{
    public class EntityTemplate
    {
        public virtual void Initialize(TransformEntity e) { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime) { }

        public void Apply(TransformEntity e)
        {
            _entity = e;

            Initialize(e);

            if (!e.HasComponent(typeof(EntityEventComponent)))
            {
                e.AddComponent(new EntityEventComponent());
            }

            var entityEvent = e.GetComponent<EntityEventComponent>();

            entityEvent.OnDraw += Draw;
            entityEvent.OnDraw += Update;
        }

        TransformEntity? _entity;
        public TransformEntity AppliedEntity
        {
            get 
            {
                Trace.Assert(_entity != null);
                return _entity;
            }
        }
    }
}