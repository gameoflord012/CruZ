using System;

using CruZ.Framework;
using CruZ.Framework.GameSystem.Render;
using CruZ.Framework.GameSystem.Render.Filters;
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
            _bloom = new BloomFilter();
            _tonemap = new TonemapFilter();
        }

        public override void Render(GameTime gameTime, SpriteBatch spriteBatch, Matrix viewProjectionMatrix)
        {
            UpdateRenderTargetsResolution();
            _gd.SetRenderTarget(_rt);
            _gd.Clear(Color.Transparent);

            // setup draw args
            DrawArgs drawArgs = new();
            drawArgs.Apply(AttachedEntity);
            drawArgs.Apply(_tex);

            // render original texture to _rt
            _fx.Parameters["view_projection"].SetValue(viewProjectionMatrix);
            spriteBatch.Begin(SpriteSortMode.Immediate, effect: _fx);
            spriteBatch.Draw(drawArgs);
            spriteBatch.End();

            // get bloom filter from _rt
            var filter = _bloom.GetFilter(_rt);

            if(Mode == 1) _gd.Clear(Color.Transparent);

            // then blending to the _rt
            if(Mode != 0)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                spriteBatch.Draw(filter, Vector2.Zero, Color.White);
                spriteBatch.End();
            }

            // get tone map filter
            _tonemap.Color = BloomColor;
            var tonemap = _tonemap.GetFilter(_rt);

            // render tone map result to screen :))
            _gd.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.Draw(tonemap, Vector2.Zero, Color.White);
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

        public float Threshold
        {
            get => _bloom.Threshold;
            set => _bloom.Threshold = value;
        }

        public Vector4 BloomColor
        {
            get;
            set;
        } = new(1, 1, 1, 1);

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

        public float[] BloomRadiuses
        {
            get => _bloom.BloomRadiuses;
        }

        Texture2D _tex;
        RenderTarget2D _rt;
        GraphicsDevice _gd;
        BloomFilter _bloom;
        TonemapFilter _tonemap;
        Effect _fx;
    }
}
