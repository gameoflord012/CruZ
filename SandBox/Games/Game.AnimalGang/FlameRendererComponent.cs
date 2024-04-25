using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem.Render;
using CruZ.GameEngine.GameSystem.Render.Filters;
using CruZ.GameEngine.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.AnimalGang.DesktopGL
{
    public class FlameRendererComponent : RendererComponent
    {
        public FlameRendererComponent()
        {
            _tex = GameContext.GameResource.Load<Texture2D>("imgs\\GAP\\Flame01.png");
            _gd = GameApplication.GetGraphicsDevice();
            _bloom = new BloomFilter();
            _quadRenderer = new QuadRenderer(_gd);

            _bloomFilterBlend = new BlendState(); // pixel = vec4(s.rgb + d.rgb, d.a)
            _bloomFilterBlend.ColorDestinationBlend = Blend.One;
            _bloomFilterBlend.AlphaDestinationBlend = Blend.DestinationAlpha;
            _bloomFilterBlend.ColorSourceBlend = Blend.One;
            _bloomFilterBlend.AlphaSourceBlend = Blend.Zero;
        }

        public override void Render(RenderSystemEventArgs e)
        {
            UpdateRenderTargetsResolution();
            _gd.SetRenderTarget(_rt);
            _gd.Clear(Color.Transparent);

            //
            // setup draw args
            //
            DrawArgs drawArgs = new();
            drawArgs.Apply(AttachedEntity);
            drawArgs.Apply(_tex);

            //
            // render original texture to _rt
            //
            var fx = EffectManager.NormalSpriteRenderer;
            fx.Parameters["view_projection"].SetValue(e.ViewProjectionMatrix);
            fx.Parameters["hdrColor"].SetValue(Vector4.One);
            e.SpriteBatch.Begin(SpriteSortMode.Immediate, effect: fx);
            e.SpriteBatch.Draw(drawArgs);
            e.SpriteBatch.End();

            if(RenderMode == RenderModes.TextureOnly) goto FINISHED;

            //
            // get bloom bloomFilter from _rt
            //
            var bloomFilter = _bloom.GetFilter(_rt);
            if (RenderMode == RenderModes.BloomOnly) _gd.Clear(Color.Transparent);

            //
            // apply bloom on _rt
            //
            e.SpriteBatch.Begin(SpriteSortMode.Immediate, _bloomFilterBlend);
            e.SpriteBatch.Draw(bloomFilter, Vector2.Zero, Color.White);
            e.SpriteBatch.End();

        FINISHED:
            //var debug = TextureHelper.GetTextureData<Vector4>(_rt);

            //
            // blend with sprite rt
            // 
            fx = EffectManager.NormalSpriteRenderer;
            fx.Parameters["view_projection"].SetValue(Matrix.Identity);
            fx.Parameters["hdrColor"].SetValue(BloomColor);
            fx.CurrentTechnique.Passes[0].Apply();
            _gd.Textures[0] = _rt;
            _gd.SetRenderTarget(e.SpriteRenderTarget);
            _gd.BlendState = BlendState.AlphaBlend;
            _quadRenderer.RenderFullScreen();
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

        public override void Dispose()
        {
            base.Dispose();

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
            get => PostProcessingSettings.MaxLuminance;
            set => PostProcessingSettings.MaxLuminance = value;
        }

        public float Brightness
        {
            get => PostProcessingSettings.Brightness;
            set => PostProcessingSettings.Brightness = value;
        }

        public Vector4 BloomColor
        {
            get;
            set;
        } = new(4f, 1.25f, 0.6f, 0.8f);

        public RenderModes RenderMode
        {
            get;
            set;
        } = RenderModes.All;

        public enum RenderModes
        {
            TextureOnly,
            BloomOnly,
            All,
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

        BlendState _bloomFilterBlend;
        QuadRenderer _quadRenderer;
    }
}
