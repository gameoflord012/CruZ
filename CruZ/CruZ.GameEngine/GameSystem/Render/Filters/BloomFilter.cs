using System;

using CruZ.Framework.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.Framework.GameSystem.Render.Filters
{
    public class BloomFilter : IDisposable
    {
        public BloomFilter()
        {
            _gd = GameApplication.GetGraphicsDevice();
            _fx = EffectManager.Bloom;
            _renderer = new QuadRenderer(_gd);

            _passTextureParam = _fx.Parameters["PassTexture"];
            _thresholdParam = _fx.Parameters["Threshold"];
            _samplingOffsetParam = _fx.Parameters["SamplingOffset"];
            _upsampleAlphaParam = _fx.Parameters["UpsampleAlpha"];

            _extractPass = _fx.CurrentTechnique.Passes[0];
            _downsamplePass = _fx.CurrentTechnique.Passes[1];
            _upsamplePass = _fx.CurrentTechnique.Passes[2];

            SelectPreset1();
        }

        private void SelectPreset1()
        {
            _thresholdParam.SetValue(0.5f);
            _bloomRadiuses = [1, 1, 1, 1, 1, 1, 1, 1];
            _bloomStrengths = [1, 1, 1, 1];
            ExitPhase = 4;
        }

        public Texture2D GetFilter(Texture2D tex)
        {
            var initialRenderTarget = _gd.GetRenderTargets();

            PrepareRenderTargets(tex);
            Texture2D resultFilter = _rtMips[0];
            //
            // Extract bright pixels
            //
            PassTexture = tex;
            _gd.SetRenderTarget(_rtMips[0]);
            _gd.BlendState = BlendState.Opaque; // override the last content
            _extractPass.Apply();
            _renderer.RenderFullScreen();

            if (ExitPhase == 0) goto FINISHED;
            //
            // Downsample
            //
            for (int i = 0; i < ExitPhase; i++)
            {
                PassTexture = _rtMips[i];
                BloomRadius = _bloomRadiuses[i];
                _gd.SetRenderTarget(_rtMips[i + 1]);
                _gd.BlendState = BlendState.Opaque; // override the last content
                _downsamplePass.Apply();
                _renderer.RenderFullScreen();
            }
            //
            // Upsample
            //
            for (int i = 0; i < ExitPhase; i++)
            {
                PassTexture = _rtMips[ExitPhase - i];
                BloomRadius = _bloomRadiuses[4 + i];
                UpsampleAlpha = 1f - _bloomStrengths[i];
                _gd.SetRenderTarget(_rtMips[ExitPhase - i - 1]);
                _gd.BlendState = BlendState.AlphaBlend; // add up to previous bloom

                _upsamplePass.Apply();
                _renderer.RenderFullScreen();
            }

        FINISHED:
            _gd.SetRenderTargets(initialRenderTarget);
            return resultFilter;
        }

        private void PrepareRenderTargets(Texture2D tex)
        {
            if (tex.Width != _width || tex.Height != _height)
            {
                _width = tex.Width;
                _height = tex.Height;

                DisposeRenderTargets();

                _rtMips[0] = new RenderTarget2D(_gd, _width, _height, true, SurfaceFormat.Vector4, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
                _rtMips[1] = new RenderTarget2D(_gd, _width / 2, _height / 2, true, SurfaceFormat.Vector4, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
                _rtMips[2] = new RenderTarget2D(_gd, _width / 4, _height / 4, true, SurfaceFormat.Vector4, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
                _rtMips[3] = new RenderTarget2D(_gd, _width / 8, _height / 8, true, SurfaceFormat.Vector4, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
                _rtMips[4] = new RenderTarget2D(_gd, _width / 16, _height / 16, true, SurfaceFormat.Vector4, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
            }
        }

        public float[] BloomStrengths { get => _bloomStrengths; }

        public float[] BloomRadiuses { get => _bloomRadiuses; }

        public float Threshold
        {
            get => _thresholdParam.GetValueSingle();
            set => _thresholdParam.SetValue(value);
        }

        public int ExitPhase = -1;

        float BloomRadius
        {
            get => _bloomRadius;
            set
            {
                if (_bloomRadius == value) return;
                _bloomRadius = value;
                UpdateSamplingOffset();
            }
        }

        float UpsampleAlpha
        {
            get => _upsampleAlphaParam.GetValueSingle();
            set => _upsampleAlphaParam.SetValue(value);
        }

        Texture2D PassTexture
        {
            get => _passTextureParam.GetValueTexture2D();
            set
            {
                if (PassTexture == value) return;
                _passTextureParam.SetValue(value);
                UpdateSamplingOffset();
            }
        }

        private void UpdateSamplingOffset()
        {
            _samplingOffsetParam.SetValue(new Vector2(
                1f / PassTexture.Width * BloomRadius,
                1f / PassTexture.Height * BloomRadius));
        }

        Effect _fx;
        GraphicsDevice _gd;
        QuadRenderer _renderer;

        EffectParameter _passTextureParam;
        EffectParameter _thresholdParam;
        EffectParameter _samplingOffsetParam;
        EffectParameter _upsampleAlphaParam;

        EffectPass _extractPass;
        EffectPass _downsamplePass;
        EffectPass _upsamplePass;

        int _width, _height;
        float _bloomRadius;

        RenderTarget2D[] _rtMips = new RenderTarget2D[5];
        float[] _bloomRadiuses = new float[8];
        float[] _bloomStrengths = new float[4];

        private void DisposeRenderTargets()
        {
            foreach (var rt in _rtMips)
            {
                rt?.Dispose();
            }
        }

        public void Dispose()
        {
            DisposeRenderTargets();
        }
    }
}
