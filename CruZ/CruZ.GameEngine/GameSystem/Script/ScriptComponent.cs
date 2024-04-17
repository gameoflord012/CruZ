using System;

using Microsoft.Xna.Framework;

namespace CruZ.GameEngine.GameSystem.Script
{
    public class ScriptComponent : Component
    {
        internal void InternalDraw(GameTime gameTime)
        {
            OnDraw(gameTime);
        }

        internal void InternalUpdate(GameTime gameTime)
        {
            OnUpdate(gameTime);
        }

        protected virtual void OnUpdate(GameTime gameTime) { }
        protected virtual void OnDraw(GameTime gameTime) { }
    }
}