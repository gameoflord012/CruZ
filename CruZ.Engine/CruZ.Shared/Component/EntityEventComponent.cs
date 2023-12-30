using Microsoft.Xna.Framework;
using System;

namespace CruZ.Components
{
    public class EntityEventComponent
    {
        public event Action<GameTime>? OnUpdate;
        public event Action<GameTime>? OnDraw;

        public void InvokeOnUpdate(GameTime gameTime)   { OnUpdate?.Invoke(gameTime); }
        public void InvokeOnDraw(GameTime gameTime)     { OnDraw?.Invoke(gameTime); }
    }
}