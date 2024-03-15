using System;
using System.Collections.Generic;

using CruZ.Common.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.Common.ECS
{
    public class LightComponent : Component
    {
        public override Type ComponentType => typeof(LightComponent);

        public List<int> SortingLayers { get; } = [];

        public LightComponent()
        {
            _lightMap = GameContext.GameResource.Load<Texture2D>("internal\\lightmap.png");
        }

        internal void InternalDraw(SpriteBatch sp, Matrix viewProjectionMat)
        {
            var fx = EffectManager.TextureLight;
            fx.Parameters["view_projection"].SetValue(viewProjectionMat);

            Vector2 pos = (Vector2)AttachedEntity.Transform.Position;
            Vector2 scale = (Vector2)AttachedEntity.Transform.Scale;
            Rectangle srcRect = _lightMap.Bounds;

            sp.Begin(effect: fx);
            sp.Draw(
                texture: _lightMap, 
                position: pos, 
                sourceRectangle: srcRect, 
                color: 
                Color.White, 
                rotation: 0,
                origin: new Vector2(srcRect.Width / 2f, srcRect.Height / 2f), 
                scale: scale, 
                SpriteEffects.None, 0);
            sp.End();
        }

        Texture2D _lightMap;
    }
}