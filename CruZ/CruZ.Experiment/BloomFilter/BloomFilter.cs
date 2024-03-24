using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace CruZ.Experiment.BloomFilter
{
    internal class BloomFilter
    {
        public BloomFilter(GraphicsDevice gd)
        {
            _renderer = new QuadRenderer();
            _gd = gd;
        }

        public void LoadContent(ContentManager content)
        {
            _fx = content.Load<Effect>("shaders\\bloom-shader");
            _screenTexture = _fx.Parameters["ScreenTexture"];
        }

        public void SetTexture(Texture2D tex)
        {
            if(_tex == tex) return;

            _tex = tex;
            _renderTarget?.Dispose();
            _renderTarget = new RenderTarget2D(_gd, tex.Width, tex.Height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
        }

        public Texture2D GetFiltered()
        {
            _gd.SetRenderTarget(_renderTarget);

            _fx.CurrentTechnique.Passes[0].Apply();
            _screenTexture.SetValue(_tex);
            _renderer.RenderQuad(_gd, -Vector2.One, Vector2.One);

            _gd.SetRenderTarget(null);
            return _renderTarget;
        }

        Texture2D _tex;
        GraphicsDevice _gd;
        Effect _fx;
        QuadRenderer _renderer;
        RenderTarget2D _renderTarget;

        EffectParameter _screenTexture;
    }
}
