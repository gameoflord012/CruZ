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

        public float LightIntensity { get; set; } = 1f;

        public LightComponent()
        {
            _lightMap = GameContext.GameResource.Load<Texture2D>("imgs\\dangcongsang.png");
            _noiseW = _noiseH = 2000;
            _perlinNoise = FunMath.GenPerlinNoise(_noiseW, _noiseH);
        }

        internal void InternalDraw(GameTime gameTime, SpriteBatch sp, Matrix viewProjectionMat)
        {
            var miliSecs = (int)gameTime.TotalGameTime.TotalMilliseconds;
            var rand = _perlinNoise[miliSecs % _noiseW, miliSecs % _noiseH];

            var fx = EffectManager.TextureLight;
            fx.Parameters["view_projection"].SetValue(viewProjectionMat);
            fx.Parameters["intensity"].SetValue(LightIntensity * rand);
            fx.Parameters["min_alpha"].SetValue(0.05f);

            Vector2 pos = AttachedEntity.Transform.Position;
            Vector2 scale = AttachedEntity.Transform.Scale;
            Rectangle srcRect = _lightMap.Bounds;

            sp.Begin(effect: fx);
            sp.Draw(
                texture: _lightMap, 
                position: pos, 
                sourceRectangle: srcRect, 
                color: 
                Color.Red, 
                rotation: 0,
                origin: new Vector2(srcRect.Width / 2f, srcRect.Height / 2f), 
                scale: scale, 
                SpriteEffects.None, 0);
            sp.End();
        }

        Texture2D _lightMap;
        float[,] _perlinNoise;
        int _noiseW, _noiseH;
    }
}