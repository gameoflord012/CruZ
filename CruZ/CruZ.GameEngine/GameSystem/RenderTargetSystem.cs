using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.GameEngine.GameSystem
{
    internal class RenderTargetSystem : EntitySystem
    {
        public override void OnInitialize()
        {
            base.OnInitialize();

            _rendererRT = GameApplication.CreateRenderTarget();
            _uiRT = GameApplication.CreateRenderTarget();
            _physicRT = GameApplication.CreateRenderTarget();
            _gd = GameApplication.GetGraphicsDevice();
            _sb = new SpriteBatch(_gd);

            _blendState = new();
            _blendState.ColorSourceBlend = Blend.SourceAlpha;
            _blendState.AlphaSourceBlend = Blend.SourceAlpha;
            _blendState.ColorDestinationBlend = Blend.InverseSourceAlpha;
            _blendState.AlphaDestinationBlend = Blend.InverseSourceAlpha;
        }

        protected override void OnDraw(SystemEventArgs args)
        {
            _gd.SetRenderTarget(null);
            _gd.Clear(GameConstants.GameBackgroundColor);

            _sb.Begin(blendState: _blendState);
            _sb.Draw(_rendererRT.Value, Vector2.Zero, Color.White);
            _sb.Draw(_physicRT.Value, Vector2.Zero, Color.White);
            _sb.Draw(_uiRT.Value, Vector2.Zero, Color.White);
            _sb.End();
        }

        private AutoResizeRenderTarget _rendererRT, _uiRT, _physicRT;
        private SpriteBatch _sb;
        private GraphicsDevice _gd;
        private BlendState _blendState;
        private bool _isDisposed;

        public override void Dispose()
        {
            base.Dispose();
            _sb.Dispose();
            _isDisposed = true;
        }

        internal static RenderTargetSystem CreateContext()
        {
            if(_instance != null && !_instance._isDisposed) throw new InvalidOperationException();
            return _instance = new RenderTargetSystem();
        }

        private static RenderTargetSystem _instance;

        public static RenderTargetSystem Instance { get => _instance ?? throw new NullReferenceException(); }
        public static RenderTarget2D RendererRT { get => _instance._rendererRT.Value; }
        public static RenderTarget2D UIRT { get => _instance._uiRT.Value; }
        public static RenderTarget2D PhysicRT { get => _instance._physicRT.Value; }
    }
}
