using System;

using Microsoft.Xna.Framework;

using Newtonsoft.Json;

namespace CruZ.Common.ECS
{
    public class EntityScript : Component
    {
        [JsonIgnore]
        public override Type ComponentType => typeof(EntityScript);

        protected override void OnAttached(TransformEntity entity)
        {
            _e = entity;
        }

        public void InternalDraw(GameTime gameTime)
        {
            OnDraw(gameTime);
        }

        public void InternalUpdate(GameTime gameTime)
        {
            OnUpdate(gameTime);
        }

        protected virtual void OnUpdate(GameTime gameTime) { }
        protected virtual void OnDraw(GameTime gameTime) { }

        protected TransformEntity AttachedEntity { get => _e; }

        TransformEntity _e;
    }
}