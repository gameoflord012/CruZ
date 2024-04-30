using System;

using Microsoft.Xna.Framework;

namespace CruZ.GameEngine.GameSystem.Script
{
    public class ScriptComponent : Component
    {
        public event Action<GameTime>? Drawing;
        public event Action<GameTime>? Updating;

        internal void InternalDraw(GameTime gameTime)
        {
            Drawing?.Invoke(gameTime);
            OnDraw(gameTime);
        }

        internal void InternalUpdate(GameTime gameTime)
        {
            Updating?.Invoke(gameTime);
            OnUpdate(gameTime);
        }

        protected virtual void OnUpdate(GameTime gameTime) { }
        protected virtual void OnDraw(GameTime gameTime) { }

        public override void Dispose()
        {
            base.Dispose();

            Drawing = default;
            Updating = default;
        }
    }
}