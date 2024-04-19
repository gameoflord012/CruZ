//using System;

//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Graphics;
//namespace CruZ.Experiment.Filter
//{
//    internal class BloomFilter
//    {
//        public BloomFilter(GraphicsDevice gd)
//        {
//            _renderer = new QuadRenderer(gd);
//            _gd = gd;
//        }

//        public void LoadContent(ContentManager content)
//        {
//            _fx = content.Load<Effect>("shaders\\bloom-shader");
//            _passTexture = _fx.Parameters["PassTexture"];
//            _originalTexture = _fx.Parameters["OriginalTexture"];
//            _bloomDistance = _fx.Parameters["BloomDistance"];
//        }

//        public void SetTexture(Texture2D tex)
//        {
//            if(_tex == tex) return;

//            _tex = tex;
//            _renderTargetEx?.Dispose();
//            //_renderTargetBlurV?.Dispose();
//            //_renderTargetBlurH?.Dispose();

//            //_renderTargetBlurV = new RenderTarget2D(_gd, tex.Width, tex.Height, true, SurfaceFormat.Vector4, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
//            //_renderTargetBlurH = new RenderTarget2D(_gd, tex.Width, tex.Height, true, SurfaceFormat.Vector4, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
//            _renderTargetEx = new RenderTarget2D(_gd, tex.Width, tex.Height, true, SurfaceFormat.Vector4, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
//            _rendterTargetBlending = new RenderTarget2D(_gd, tex.Width, tex.Height, true, SurfaceFormat.Vector4, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
//        }

//        public Texture2D GetFiltered()
//        {
//            _bloomDistance.SetValue(2f);
//            _originalTexture.SetValue(_tex);
//            _passTexture.SetValue(_tex);
//            _gd.SetRenderTarget(_renderTargetEx);
//            _fx.Techniques["ExtractBlur"].Passes[0].Apply();
//            _renderer.RenderQuad(_gd, -Vector2.One, Vector2.One);

//            _passTexture.SetValue(_renderTargetEx);
//            _gd.SetRenderTarget(_renderTargetBlurV);
//            _fx.Techniques["ExtractBlur"].Passes[1].Apply(); // Blur Vertical
//            _renderer.RenderQuad(_gd, -Vector2.One, Vector2.One);

//            _passTexture.SetValue(_renderTargetBlurV);
//            _gd.SetRenderTarget(_renderTargetBlurH);
//            _fx.Techniques["ExtractBlur"].Passes[2].Apply(); // Blur Horizontal
//            _renderer.RenderQuad(_gd, -Vector2.One, Vector2.One);

//            _passTexture.SetValue(_renderTargetBlurH);
//            _gd.SetRenderTarget(_rendterTargetBlending);
//            _fx.Techniques["Blending"].Passes[0].Apply(); // Blur Horizontal
//            _renderer.RenderQuad(_gd, -Vector2.One, Vector2.One);

//            _gd.SetRenderTarget(null);
//            return _rendterTargetBlending;
//        }

//        Texture2D _tex;
//        GraphicsDevice _gd;
//        Effect _fx;
//        QuadRenderer _renderer;

//        RenderTarget2D _renderTargetEx;
//        RenderTarget2D _rendterTargetBlending;

//        EffectParameter _passTexture;
//        EffectParameter _originalTexture;
//        EffectParameter _bloomDistance;
//    }
//}
