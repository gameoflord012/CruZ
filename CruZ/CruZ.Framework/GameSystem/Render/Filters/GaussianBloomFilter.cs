using System;

using CruZ.Framework.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace CruZ.Framework.GameSystem.Render
{
    public class GuassianBloomFilter : IDisposable
    {
        public GuassianBloomFilter(GraphicsDevice gd)
        {
            _gd = gd;
            _fx = EffectManager.GaussianBloom;
            _renderer = new QuadRenderer(gd);

            _passTextureParam = _fx.Parameters["PassTexture"];
            _thresholdParam = _fx.Parameters["Threshold"];
            _intensityParam = _fx.Parameters["Intensity"];
            _colorParam = _fx.Parameters["Color"];
            _samplingOffset = _fx.Parameters["SamplingOffset"];

            _extractPass = _fx.CurrentTechnique.Passes[0];
            _downsamplePass = _fx.CurrentTechnique.Passes[1];
            _upsamplePass = _fx.CurrentTechnique.Passes[2];

            _overrideBlend = new BlendState();
            _overrideBlend.AlphaSourceBlend = Blend.One;
            _overrideBlend.ColorSourceBlend = Blend.One;
            _overrideBlend.AlphaDestinationBlend = Blend.Zero;
            _overrideBlend.ColorDestinationBlend = Blend.Zero;

            ChoosePreset1();
        }

        public Texture2D GetFilter(Texture2D tex, float resolutionScale = 1)
        {
            Texture2D resultFilter;
            var initialRenderTarget = _gd.GetRenderTargets();

            PrepareRenderTargets(tex, resolutionScale);
            _gd.BlendState = _overrideBlend;
            _samplingOffset.SetValue(new Vector2(
                1f / tex.Width * Radius, 
                1f / tex.Height * Radius));
            resultFilter = _rtMip0;

            //
            // Extract bright pixels
            //
            _passTextureParam.SetValue(tex);
            _gd.SetRenderTarget(_rtMip0);
            _extractPass.Apply(); 
            _renderer.RenderQuad(_gd, -Vector2.One, Vector2.One);

            if(ExitPhase == 0) goto FINISHED;

            //
            // Downsample
            //
            _passTextureParam.SetValue(_rtMip0);
            _gd.SetRenderTarget(_rtMip1);
            _downsamplePass.Apply(); // Blur Vertical
            _renderer.RenderQuad(_gd, -Vector2.One, Vector2.One);

            //
            // Upsample
            //
            _passTextureParam.SetValue(_rtMip1);
            _gd.SetRenderTarget(_rtMip0);
            _upsamplePass.Apply(); // Upsample
            _renderer.RenderQuad(_gd, -Vector2.One, Vector2.One);

            var debug = GetTextureData<Vector4>(resultFilter);

        FINISHED:
            _gd.SetRenderTargets(initialRenderTarget);
            return resultFilter;
        }

        private T[] GetTextureData<T>(Texture2D tex) where T : struct
        {
            T[] data = new T[tex.Width * tex.Height];
            tex.GetData(data);
            return data;
        }

        private void PrepareRenderTargets(Texture2D tex, float resolutionScale)
        {
            var scaledWidth = (int)(tex.Width * resolutionScale);
            var scaledHeight = (int)(tex.Height * resolutionScale);

            if (scaledWidth != _width || scaledHeight != _height)
            {
                _width = scaledWidth;
                _height = scaledHeight;

                DisposeRenderTargets();

                _rtMip0 = new RenderTarget2D(_gd, tex.Width, tex.Height, false, SurfaceFormat.Vector4, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
                _rtMip1 = new RenderTarget2D(_gd, _width, _height, false, SurfaceFormat.Vector4, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
            }
        }

        private void ChoosePreset1()
        {
            _intensityParam.SetValue(1f);
            _thresholdParam.SetValue(0.5f);
            _colorParam.SetValue(new Vector4(1, 1, 1, 1));
            Radius = 2f;
        }

        public float Threshold
        {
            get => _thresholdParam.GetValueSingle();
            set => _thresholdParam.SetValue(value);
        }

        public float Intensity
        {
            get => _intensityParam.GetValueSingle();
            set => _intensityParam.SetValue(value);
        }

        public Vector4 BlendColor
        {
            get => _colorParam.GetValueVector4();
            set => _colorParam.SetValue(value);
        }

        public int ExitPhase = -1;

        public float Radius = 1f;

        //public float WeightMultiplier
        //{
        //    get => _weightMultiplierParam.GetValueSingle();
        //    set => _weightMultiplierParam.SetValue(value);
        //}

        Effect _fx;
        GraphicsDevice _gd;
        QuadRenderer _renderer;

        EffectParameter _passTextureParam;
        EffectParameter _thresholdParam;
        EffectParameter _intensityParam;
        EffectParameter _colorParam;
        EffectParameter _samplingOffset;

        EffectPass _extractPass;
        EffectPass _downsamplePass;
        EffectPass _upsamplePass;

        int _width, _height;

        RenderTarget2D _rtMip0;
        RenderTarget2D _rtMip1;

        BlendState _overrideBlend;

        private void DisposeRenderTargets()
        {
            _rtMip0?.Dispose();
            _rtMip1?.Dispose();
        }

        public void Dispose()
        {
            DisposeRenderTargets();
        }
    }
}
