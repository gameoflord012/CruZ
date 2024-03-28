using System;

using CruZ.Framework.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
            _originalTextureParam = _fx.Parameters["OriginalTexture"];
            _thresholdParam = _fx.Parameters["Threshold"];
            _exposureParam = _fx.Parameters["Exposure"];
            _colorParam = _fx.Parameters["Color"];

            ChoosePreset1();
        }

        public Texture2D GetFilter(Texture2D tex, float resolutionMultiplier = 1)
        {
            PrepareRenderTargets(tex);
            var initialRenderTarget = _gd.GetRenderTargets();

            Vector4[] debug = new Vector4[_width * _height];

            _gd.BlendState = BlendState.Opaque;

            _originalTextureParam.SetValue(tex);

            _passTextureParam.SetValue(tex);
            _gd.SetRenderTarget(_rtEx);
            _fx.CurrentTechnique.Passes[0].Apply(); // Xtract pass
            _renderer.RenderQuad(_gd, -Vector2.One, Vector2.One);

            var rtLastBlur = _rtEx;
            for (int i = 0; i < BlurCount; i++)
            {
                _passTextureParam.SetValue(rtLastBlur);
                _gd.SetRenderTarget(_rtBlurV);
                _fx.CurrentTechnique.Passes[1].Apply(); // Blur Vertical
                _renderer.RenderQuad(_gd, -Vector2.One, Vector2.One);

                _rtBlurV.GetData(debug);

                _passTextureParam.SetValue(_rtBlurV);
                _gd.SetRenderTarget(_rtBlurH);
                _fx.CurrentTechnique.Passes[2].Apply(); // Blur Horizontal
                _renderer.RenderQuad(_gd, -Vector2.One, Vector2.One);

                _rtBlurH.GetData(debug);

                rtLastBlur = _rtBlurH;
            }

            if(ShouldBlend)
            {
                _passTextureParam.SetValue(_rtBlurH);
                _gd.SetRenderTarget(_rtBlend);
                _fx.CurrentTechnique.Passes[3].Apply(); // Blend
                _renderer.RenderQuad(_gd, -Vector2.One, Vector2.One);
            }
            
            _gd.SetRenderTargets(initialRenderTarget);
            return ShouldBlend ? _rtBlend : _rtBlurH;
        }

        private void PrepareRenderTargets(Texture2D tex, float )
        {
            if (tex.Width != _width || tex.Height != _height)
            {
                _width = tex.Width;
                _height = tex.Height;

                _rtEx?.Dispose();
                _rtBlurV?.Dispose();
                _rtBlurH?.Dispose();
                _rtBlend?.Dispose();

                _rtEx = new RenderTarget2D(_gd, _width, _height, false, SurfaceFormat.Vector4, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
                _rtBlurV = new RenderTarget2D(_gd, _width, _height, false, SurfaceFormat.Vector4, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
                _rtBlurH = new RenderTarget2D(_gd, _width, _height, false, SurfaceFormat.Vector4, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
                _rtBlend = new RenderTarget2D(_gd, _width, _height, false, SurfaceFormat.Vector4, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);            }
        }

        private void ChoosePreset1()
        {
            _exposureParam.SetValue(5f);
            _thresholdParam.SetValue(0.5f);
            _colorParam.SetValue(new Vector4(1, 1, 1, 1));
            BlurCount = 5;
        }
        
        public float Threshold
        {
            get => _thresholdParam.GetValueSingle();
            set => _thresholdParam.SetValue(value);
        }

        public float Exposure
        {
            get => _exposureParam.GetValueSingle();
            set => _exposureParam.SetValue(value);
        }

        public Vector4 Color
        {
            get => _colorParam.GetValueVector4();
            set => _colorParam.SetValue(value);
        }

        public bool ShouldBlend = true;

        public int BlurCount = 5;

        Effect _fx;
        GraphicsDevice _gd;
        QuadRenderer _renderer;

        RenderTarget2D _rtEx;
        RenderTarget2D _rtBlurV;
        RenderTarget2D _rtBlurH;
        RenderTarget2D _rtBlend;

        EffectParameter _passTextureParam;
        EffectParameter _originalTextureParam;
        EffectParameter _thresholdParam;
        EffectParameter _exposureParam;
        EffectParameter _colorParam;

        int _width, _height;

        public void Dispose()
        {
            _rtEx.Dispose();
            _rtBlurV.Dispose();
            _rtBlurH.Dispose();
        }
    }
}
