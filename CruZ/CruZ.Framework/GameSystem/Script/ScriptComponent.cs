using System;

using CruZ.Framework.GameSystem.ECS;

using Microsoft.Xna.Framework;

using Newtonsoft.Json;

namespace CruZ.Framework.GameSystem.Script
{
    public class ScriptComponent : Component
    {
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