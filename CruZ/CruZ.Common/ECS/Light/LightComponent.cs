using System;
using System.Collections.Generic;
using System.Resources;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.Common.ECS
{
    public class LightComponent : Component
    {
        public override Type ComponentType => typeof(LightComponent);

        public List<int> SortingLayers { get; } = [];

        protected override void Initialize()
        {
            base.Initialize();

            _lightMap = GameContext.GameResource.Load<Texture2D>("internal\\lightmap.png");
        }

        internal void InternalDraw(SpriteBatch sp, Effect fx)
        {
            Vector2 position = new(
                AttachedEntity.Transform.Position.X, 
                AttachedEntity.Transform.Position.Y);

            sp.Begin(effect: fx);
            sp.Draw(_lightMap, position, Color.White);
            sp.End();
        }

        Texture2D _lightMap;
    }
}