using System;

using CruZ.Framework;
using CruZ.Framework.GameSystem.Render;
using CruZ.Framework.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.AnimalGang.DesktopGL
{
    public class FlameRendererComponent : RendererComponent
    {
        public FlameRendererComponent()
        {
            _fx = EffectManager.NormalSpriteRenderer;
            _tex = GameContext.GameResource.Load<Texture2D>("imgs\\GAP\\Flame01.png");
            _gd = GameApplication.GetGraphicsDevice();
            _bloom = new GuassianBloomFilter(_gd);

            _additiveBlend = new BlendState();
            _additiveBlend.AlphaSourceBlend = Blend.Zero;
            _additiveBlend.ColorSourceBlend = Blend.One;
            _additiveBlend.AlphaDestinationBlend = Blend.DestinationAlpha;
            _additiveBlend.ColorDestinationBlend = Blend.DestinationAlpha;
        }

        public override void Render(GameTime gameTime, SpriteBatch spriteBatch, Matrix viewProjectionMatrix)
        {
            UpdateRenderTargetsResolution();
            _gd.SetRenderTarget(_rt);
            _gd.Clear(Color.Transparent);

            // setup draw args for the original texture
            DrawArgs drawArgs = new();
            drawArgs.Apply(AttachedEntity);
            drawArgs.Apply(_tex);

            // render original texture to a render target
            _fx.Parameters["view_projection"].SetValue(viewProjectionMatrix);
            spriteBatch.Begin(SpriteSortMode.Immediate, effect: _fx);
            spriteBatch.Draw(drawArgs);
            spriteBatch.End();

            // apply bloom on that render target
            var filter = _bloom.GetFilter(_rt, ResolutionScale);

            // then additive blending
            spriteBatch.Begin(SpriteSortMode.Immediate, _additiveBlend);
            spriteBatch.Draw(filter, Vector2.Zero, Color.White);
            spriteBatch.End();

            // render the rt to screen :))
            _gd.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.Draw(_rt, Vector2.Zero, Color.White);
            spriteBatch.End();
        }

        private void UpdateRenderTargetsResolution()
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

        public float Intensity
        {
            get => _bloom.Intensity;
            set => _bloom.Intensity = value;
        }

        public float Threshold
        {
            get => _bloom.Threshold;
            set => _bloom.Threshold = value;
        }

        public float ResolutionScale
        {
            get;
            set;
        } = 0.5f;

        public Vector4 BloomColor
        {
            get => _bloom.BlendColor;
            set => _bloom.BlendColor = value;
        }

        /// <summary>
        /// Mode 0: Show texture only <br/>
        /// Mode 1: Show filter only
        /// </summary>
        public int Mode
        {
            get;
            set;
        } = -1;

        public int BloomPhase
        {
            get => _bloom.ExitPhase;
            set => _bloom.ExitPhase = value;
        }

        //public float Stride
        //{
        //    get => _bloom.Radius;
        //    set => _bloom.Radius = value;
        //}

        //public float WeightMultiplier
        //{
        //    get => _bloom.WeightMultiplier;
        //    set => _bloom.WeightMultiplier = value;
        //}

        Texture2D _tex;
        RenderTarget2D _rt;
        GraphicsDevice _gd;
        GuassianBloomFilter _bloom;
        Effect _fx;

        BlendState _additiveBlend;
    }
}
