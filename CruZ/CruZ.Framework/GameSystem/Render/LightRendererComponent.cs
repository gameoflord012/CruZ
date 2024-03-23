using System;
using System.Collections.Generic;

using CruZ.Framework.UI;
using CruZ.Framework.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.Framework.GameSystem.Render
{
    public class LightRendererComponent : RendererComponent, IHasBoundBox
    {
        public event Action<UIBoundingBox> BoundingBoxChanged;
     
        public LightRendererComponent()
        {
            _lightMap = GameContext.GameResource.Load<Texture2D>("imgs\\dangcongsang.png");
            _noiseW = _noiseH = 2000;
            _perlinNoise = FunMath.GenPerlinNoise(_noiseW, _noiseH);
        }

        public override void Render(GameTime gameTime, SpriteBatch spriteBatch, Matrix viewProjectionMatrix)
        {
            var miliSecs = (int)gameTime.TotalGameTime.TotalMilliseconds;
            var rand = _perlinNoise[miliSecs % _noiseW, miliSecs % _noiseH];

            var fx = EffectManager.TextureLight;
            fx.Parameters["view_projection"].SetValue(viewProjectionMatrix);
            fx.Parameters["intensity"].SetValue(LightIntensity * rand);
            fx.Parameters["min_alpha"].SetValue(0.05f);

            DrawArgs drawArgs = new();
            drawArgs.Apply(AttachedEntity);
            drawArgs.Apply(_lightMap);

            spriteBatch.Begin(effect: fx);
            spriteBatch.Draw(drawArgs);
            spriteBatch.End();
        }

        public float LightIntensity { get; set; }

        Texture2D _lightMap;
        float[,] _perlinNoise;
        int _noiseW, _noiseH;
    }
}