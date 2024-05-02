using System;

using CruZ.GameEngine.GameSystem.UI;
using CruZ.GameEngine.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.GameEngine.GameSystem.Render
{
    public class LightRendererComponent : RendererComponent, IRectUIProvider
    {
        public event Action<RectUIInfo> UIRectChanged;

        public LightRendererComponent()
        {
            _lightMap = GameApplication.Resource.Load<Texture2D>("imgs\\dangcongsang.png");
            _noiseW = _noiseH = 2000;
            _perlinNoise = FunMath.GenPerlinNoise(_noiseW, _noiseH);
        }

        public override void Render(RenderSystemEventArgs e)
        {
            var miliSecs = (int)e.GameTime.TotalGameTime.TotalMilliseconds;
            var rand = _perlinNoise[miliSecs % _noiseW, miliSecs % _noiseH];

            var fx = EffectManager.TextureLight;
            fx.Parameters["view_projection"].SetValue(e.ViewProjectionMatrix);
            fx.Parameters["intensity"].SetValue(LightIntensity * rand);
            fx.Parameters["min_alpha"].SetValue(0.05f);

            SpriteDrawArgs drawArgs = new();
            drawArgs.Apply(AttachedEntity.Transform);
            drawArgs.Apply(_lightMap);

            e.SpriteBatch.Begin(effect: fx);
            e.SpriteBatch.DrawWorld(drawArgs);
            e.SpriteBatch.End();

            UIRectChanged?.Invoke(new RectUIInfo(
                drawArgs.GetWorldBounds(), [drawArgs.GetWorldOrigin()]));
        }

        public float LightIntensity { get; set; }

        Texture2D _lightMap;
        float[,] _perlinNoise;
        int _noiseW, _noiseH;
    }
}