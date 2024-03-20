using System;
using System.Collections.Generic;

using CruZ.Common.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.Common.ECS
{
    public class LightRendererComponent : RendererComponent
    {
        public float LightIntensity { get; set; }

        public LightRendererComponent()
        {
            _lightMap = GameContext.GameResource.Load<Texture2D>("imgs\\dangcongsang.png");
            _noiseW = _noiseH = 2000;
            _perlinNoise = FunMath.GenPerlinNoise(_noiseW, _noiseH);
        }

        internal override void Render(GameTime gameTime, SpriteBatch spriteBatch, Matrix viewProjectionMatrix)
        {
            var miliSecs = (int)gameTime.TotalGameTime.TotalMilliseconds;
            var rand = _perlinNoise[miliSecs % _noiseW, miliSecs % _noiseH];

            var fx = EffectManager.TextureLight;
            fx.Parameters["view_projection"].SetValue(viewProjectionMatrix);
            fx.Parameters["intensity"].SetValue(LightIntensity * rand);
            fx.Parameters["min_alpha"].SetValue(0.05f);

            Vector2 pos = AttachedEntity.Transform.Position;
            Vector2 scale = AttachedEntity.Transform.Scale;
            Rectangle srcRect = _lightMap.Bounds;

            spriteBatch.Begin(effect: fx);
            spriteBatch.Draw(
                texture: _lightMap, 
                position: pos, 
                sourceRectangle: srcRect, 
                color: 
                Color.Red, 
                rotation: 0,
                origin: new Vector2(srcRect.Width / 2f, srcRect.Height / 2f), 
                scale: scale, 
                SpriteEffects.None, 0);
            spriteBatch.End();
        }

        Texture2D _lightMap;
        float[,] _perlinNoise;
        int _noiseW, _noiseH;
    }
}