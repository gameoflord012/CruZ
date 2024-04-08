using CruZ.Framework.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.Framework.GameSystem.Render.Filters
{
    public class TonemapFilter
    {
        public TonemapFilter()
        {
            _gd = GameApplication.GetGraphicsDevice();
            _quadRenderer = new(_gd);
            _fx = EffectManager.ReinhardTonemap;
        }

        public Texture2D GetFilter(Texture2D tex)
        {
            PrepareRenderTarget(tex);

            _fx.CurrentTechnique.Passes[0].Apply();
            _fx.Parameters["ScreenTexture"].SetValue(tex);
            _fx.Parameters["MaxLuminance"].SetValue(MaxLuminance);
            //_fx.Parameters["Color"].SetValue(HdrColor);
            _gd.SetRenderTarget(_rt);
            _gd.BlendState = BlendState.Opaque;
            _quadRenderer.RenderFullScreen();

            return _rt;
        }

        private void PrepareRenderTarget(Texture2D tex)
        {
            if(_rt == null || _width != tex.Width || _height != tex.Height)
            {
                _width = tex.Width;
                _height = tex.Height;

                _rt?.Dispose();
                _rt = new RenderTarget2D(_gd, _width, _height, false, SurfaceFormat.Vector4, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
            }
        }

        public float MaxLuminance = 1;

        //public Vector4 HdrColor = Vector4.One;

        QuadRenderer _quadRenderer;
        RenderTarget2D? _rt;
        GraphicsDevice _gd;
        Effect _fx;

        int _width, _height;
    }
}
