using System;

using CruZ.Framework;
using CruZ.Framework.GameSystem.Render;
using CruZ.Framework.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SharpDX.Direct2D1.Effects;

namespace Game.AnimalGang.DesktopGL
{
    public class FlameRendererComponent : RendererComponent
    {
        public FlameRendererComponent()
        {
            _fx = EffectManager.NormalSpriteRenderer;
            _tex = GameContext.GameResource.Load<Texture2D>("imgs\\homelander.jpg");
            _gd = GameApplication.GetGraphicsDevice();
            _bloom = new GuassianBloomFilter(_gd);
        }

        public override void Render(GameTime gameTime, SpriteBatch spriteBatch, Matrix viewProjectionMatrix)
        {
            // prepare filtering render target
            UpdateResolution();
            _gd.SetRenderTarget(_rt);
            _gd.Clear(Color.Transparent);

            // setup draw args
            DrawArgs drawArgs = new();
            drawArgs.Apply(AttachedEntity);
            drawArgs.Apply(_tex);

            // render original texture on filtering render target
            _fx.Parameters["view_projection"].SetValue(viewProjectionMatrix);
            spriteBatch.Begin(SpriteSortMode.Immediate, effect: _fx);
            spriteBatch.Draw(drawArgs);
            spriteBatch.End();

            // apply bloom on that render target
            var filter = _bloom.GetFilter(_rt);

            // render the filter to screen :))
            _gd.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.Draw(filter, Vector2.Zero, Color.White);
            spriteBatch.End();
        }

        private void UpdateResolution()
        {
            if (_rt == null || _gd.Viewport.Width != _rt.Width || _gd.Viewport.Height != _rt.Height)
            {
                _rt?.Dispose();
                _rt = new RenderTarget2D(_gd, _gd.Viewport.Width, _gd.Viewport.Height,
                    false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
            }
        }


        protected override void OnDispose()
        {
            base.OnDispose();

            _rt.Dispose();
            _bloom.Dispose();
        }

        public float Exposure
        {
            get => _bloom.Exposure;
            set => _bloom.Exposure = value;
        }

        public float Threshold
        {
            get => _bloom.Threshold;
            set => _bloom.Threshold = value;
        }

        public Vector4 BloomColor
        {
            get => _bloom.Color;
            set => _bloom.Color = value;
        }

        public bool ShouldBlend
        {
            get => _bloom.ShouldBlend;
            set => _bloom.ShouldBlend = value;
        }

        public int BlurCount
        {
            get => _bloom.BlurCount;
            set => _bloom.BlurCount = value;
        }

        Texture2D _tex;
        RenderTarget2D _rt;
        GraphicsDevice _gd;
        GuassianBloomFilter _bloom;
        Effect _fx;
    }
}
