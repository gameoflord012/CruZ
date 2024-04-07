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

            _bloomFilterBlend = new BlendState(); // pixel = vec4(s.rgb + d.rgb, d.a)
            _bloomFilterBlend.ColorDestinationBlend = Blend.One;
            _bloomFilterBlend.AlphaDestinationBlend = Blend.DestinationAlpha;
            _bloomFilterBlend.ColorSourceBlend = Blend.One;
            _bloomFilterBlend.AlphaSourceBlend = Blend.Zero;
        }

        public override void Render(GameTime gameTime, SpriteBatch spriteBatch, Matrix viewProjectionMatrix)
        {
            UpdateRenderTargetsResolution();
            _gd.SetRenderTarget(_rt);
            _gd.Clear(Color.Transparent);

            Texture2D finalTex = _rt;

            // setup draw args
            DrawArgs drawArgs = new();
            drawArgs.Apply(AttachedEntity);
            drawArgs.Apply(_tex);

            // render original texture to _rt
            _fx.Parameters["view_projection"].SetValue(viewProjectionMatrix);
            spriteBatch.Begin(SpriteSortMode.Immediate, effect: _fx);
            spriteBatch.Draw(drawArgs);
            spriteBatch.End();

            var debug = TextureHelper.GetTextureData<Vector4>(_rt);
            if(RenderMode == RenderModes.Texture) goto FINISHED;

            // get bloom bloomFilter from _rt
            var bloomFilter = _bloom.GetFilter(_rt);
            if (RenderMode == RenderModes.BloomFilter) _gd.Clear(Color.Transparent);

            // apply bloom on _rt
            spriteBatch.Begin(SpriteSortMode.Immediate, _bloomFilterBlend);
            spriteBatch.Draw(bloomFilter, Vector2.Zero, Color.White);
            spriteBatch.End();

            debug = TextureHelper.GetTextureData<Vector4>(_rt);
            debug = TextureHelper.GetTextureData<Vector4>(bloomFilter);
            if (RenderMode == RenderModes.BloomFilter) goto FINISHED;

            //get tone map bloomFilter
            _tonemap.Color = BloomColor;
            var tonemap = _tonemap.GetFilter(_rt);
            finalTex = tonemap;

            debug = TextureHelper.GetTextureData<Vector4>(tonemap);

        FINISHED:
            _gd.SetRenderTarget(null);
            if(BackgroundMode == BackgroundModes.Black) _gd.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Immediate);
            spriteBatch.Draw(finalTex, Vector2.Zero, Color.White);
            spriteBatch.End();
        }

        private void UpdateRenderTargetsResolution()
        {
            if (_rt == null || _gd.Viewport.Width != _rt.Width || _gd.Viewport.Height != _rt.Height)
            {
                _rt?.Dispose();
                _rt = new RenderTarget2D(_gd, _gd.Viewport.Width, _gd.Viewport.Height,
                    false, SurfaceFormat.Vector4, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
            }
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            _rt.Dispose();
            _bloom.Dispose();
            _bloomFilterBlend.Dispose();
        }

        public float Threshold
        {
            get => _bloom.Threshold;
            set => _bloom.Threshold = value;
        }

        public float MaxLuminance
        {
            get => _tonemap.MaxLuminance;
            set => _tonemap.MaxLuminance = value;
        }

        public Vector4 BloomColor
        {
            get;
            set;
        } = new(1, 1, 1, 1);

        /// <summary>
        /// Mode 0: Show texture only <br/>
        /// Mode 1: Show bloomFilter only
        /// </summary>
        public RenderModes RenderMode
        {
            get;
            set;
        } = RenderModes.Tonemap;

        public BackgroundModes BackgroundMode
        {
            get;
            set;
        } = BackgroundModes.BackBuffer;

        public enum RenderModes
        {
            Texture,
            BloomFilter,
            Tonemap
        }

        public enum BackgroundModes
        {
            Black,
            BackBuffer
        }

        public int BloomPhase
        {
            get => _bloom.ExitPhase;
            set => _bloom.ExitPhase = value;
        }

        public float[] BloomRadiuses
        {
            get => _bloom.BloomRadiuses;
        }

        public float[] BloomStrengths
        {
            get => _bloom.BloomStrengths;
        }

        Texture2D _tex;
        RenderTarget2D _rt;
        GraphicsDevice _gd;
        BloomFilter _bloom;
        TonemapFilter _tonemap;
        Effect _fx;

        BlendState _bloomFilterBlend;
    }
}
