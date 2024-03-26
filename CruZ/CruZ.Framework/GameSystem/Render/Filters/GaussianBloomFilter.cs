using System;

using CruZ.Framework.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace CruZ.Framework.GameSystem.Render
{
    internal class GuassianBloomFilter : IDisposable
    {
        public GuassianBloomFilter(GraphicsDevice gd)
        {
            _gd = gd;
            _fx = EffectManager.GaussianBloom;
            _renderer = new QuadRenderer(gd);

            _passTextureParam = _fx.Parameters["PassTexture"];
            _bloomDistanceParam = _fx.Parameters["BloomDistance"];
        }

        public Texture2D GetFilter(Texture2D tex)
        {
            PrepareRenderTargets(tex);

            _bloomDistanceParam.SetValue(2f);
            _passTextureParam.SetValue(tex);
            _gd.SetRenderTarget(_renderTargetEx);
            _fx.Techniques["ExtractBlur"].Passes[0].Apply();
            _renderer.RenderQuad(_gd, -Vector2.One, Vector2.One);

            _passTextureParam.SetValue(_renderTargetEx);
            _gd.SetRenderTarget(_renderTargetBlurV);
            _fx.Techniques["ExtractBlur"].Passes[1].Apply(); // Blur Vertical
            _renderer.RenderQuad(_gd, -Vector2.One, Vector2.One);

            _passTextureParam.SetValue(_renderTargetBlurV);
            _gd.SetRenderTarget(_renderTargetBlurH);
            _fx.Techniques["ExtractBlur"].Passes[2].Apply(); // Blur Horizontal
            _renderer.RenderQuad(_gd, -Vector2.One, Vector2.One);

            return _renderTargetBlurH;
        }

        private void PrepareRenderTargets(Texture2D tex)
        {
            if (tex.Width == _width || tex.Height == _height) return;
            _width = tex.Width;
            _height = tex.Height;

            _renderTargetEx?.Dispose();
            _renderTargetBlurV?.Dispose();
            _renderTargetBlurH?.Dispose();

            _renderTargetEx = new RenderTarget2D(_gd, _width, _height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
            _renderTargetBlurV = new RenderTarget2D(_gd, _width, _height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
            _renderTargetBlurH = new RenderTarget2D(_gd, _width, _height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
        }

        public float BloomDistance 
        { 
            get => _bloomDistanceParam.GetValueSingle(); 
            set => _bloomDistanceParam.SetValue(value); 
        }

        GraphicsDevice _gd;
        Effect _fx;
        QuadRenderer _renderer;
        RenderTarget2D _renderTargetEx;
        RenderTarget2D _renderTargetBlurV;
        RenderTarget2D _renderTargetBlurH;

        EffectParameter _passTextureParam;
        EffectParameter _bloomDistanceParam;

        int _width, _height;

        public void Dispose()
        {
            _renderTargetEx.Dispose();
            _renderTargetBlurV.Dispose();
            _renderTargetBlurH.Dispose();
        }
    }
}
