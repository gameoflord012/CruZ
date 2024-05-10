using System;

using Microsoft.Xna.Framework;

namespace CruZ.GameEngine.GameSystem.Script
{
    public class ScriptComponent : Component
    {
        public event Action<ScriptUpdateArgs>? Updating;
        public event Action<GameTime>? Drawing;

        internal void InternalDraw(GameTime gameTime)
        {
            Drawing?.Invoke(gameTime);
            OnDraw(gameTime);
        }

        internal void InternalUpdate(ScriptUpdateArgs scriptUpdateArgs)
        {
            Updating?.Invoke(scriptUpdateArgs);
            OnUpdate(scriptUpdateArgs);
        }

        protected virtual void OnUpdate(ScriptUpdateArgs scriptUpdateArgs)
        {

        }

        protected virtual void OnDraw(GameTime gameTime)
        { }

        public override void Dispose()
        {
            base.Dispose();

            Drawing = default;
            Updating = default;
        }
    }
}
