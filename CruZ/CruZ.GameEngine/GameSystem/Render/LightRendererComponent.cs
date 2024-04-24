using System;

using CruZ.GameEngine.GameSystem.UI;
using CruZ.GameEngine.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.GameEngine.GameSystem.Render
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

        public override void Render(RenderSystemEventArgs e)
        {
            var miliSecs = (int)e.GameTime.TotalGameTime.TotalMilliseconds;
            var rand = _perlinNoise[miliSecs % _noiseW, miliSecs % _noiseH];

            var fx = EffectManager.TextureLight;
            fx.Parameters["view_projection"].SetValue(e.ViewProjectionMatrix);
            fx.Parameters["intensity"].SetValue(LightIntensity * rand);
            fx.Parameters["min_alpha"].SetValue(0.05f);

            DrawArgs drawArgs = new();
            drawArgs.Apply(AttachedEntity);
            drawArgs.Apply(_lightMap);

            e.SpriteBatch.Begin(effect: fx);
            e.SpriteBatch.Draw(drawArgs);
            e.SpriteBatch.End();

            BoundingBoxChanged?.Invoke(new UIBoundingBox(
                drawArgs.GetWorldBounds(), [drawArgs.GetWorldOrigin()]));
        }

        public float LightIntensity { get; set; }

        Texture2D _lightMap;
        float[,] _perlinNoise;
        int _noiseW, _noiseH;
    }
}