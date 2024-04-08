using System;
using System.Diagnostics;
using System.Linq;

using CruZ.Common;
using CruZ.Framework.GameSystem.ECS;
using CruZ.Framework.GameSystem.Render.Filters;
using CruZ.Framework.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.Framework.GameSystem.Render
{
    internal class RenderSystem : EntitySystem
    {
        public override void Initialize()
        {
            _gd = GameApplication.GetGraphicsDevice();
            _spriteBatch = new SpriteBatch(_gd);
            _tonemap = new TonemapFilter();
            UpdateRenderTargetResolution(_gd.Viewport);
            GameApplication.WindowResized += UpdateRenderTargetResolution;
        }

        private void UpdateRenderTargetResolution(Viewport vp)
        {
            _rtSprite = new RenderTarget2D(_gd, _gd.Viewport.Width, _gd.Viewport.Height, false, SurfaceFormat.Vector4, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
        }

        protected override void OnDraw(EntitySystemEventArgs args)
        {
            var rendererArgs = new RendererEventArgs(
                args.GameTime, 
                _spriteBatch,
                Camera.Main.ViewProjectionMatrix(),
                _rtSprite);

            var renderers = args.Entity.GetAllComponents()
                .Where(e => e is RendererComponent)
                .Select(e => (RendererComponent)e)
                .ToList();

            renderers.Sort();

            _gd.SetRenderTarget(_rtSprite);
            //_gd.Clear(Color.White);
            _gd.Clear(GameConstants.GAME_BACKGROUND_COLOR);

            foreach (var renderer in renderers)
            {
                _gd.SetRenderTarget(_rtSprite); // make sure the rt is unchanged 
                renderer.Render(rendererArgs);
            }

            //
            // post processing
            //
            _tonemap.MaxLuminance = MaxLuminance;
            var tonemapFilter = _tonemap.GetFilter(_rtSprite);
            var debug = TextureHelper.GetTextureData<Vector4>(_rtSprite);
            debug = TextureHelper.GetTextureData<Vector4>(tonemapFilter);

            //
            //render to back buffer
            //
            _gd.SetRenderTarget(null);
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            _spriteBatch.Draw(tonemapFilter, Vector2.Zero, Color.White);
            _spriteBatch.End();
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            _rtSprite.Dispose();
        }

        RenderTarget2D _rtSprite;
        SpriteBatch _spriteBatch;
        GraphicsDevice _gd;
        TonemapFilter _tonemap;

        public static float MaxLuminance = 1;
    }

    public static class PostProcessingSettings
    {
        public static float MaxLuminance
        {
            get => RenderSystem.MaxLuminance;
            set => RenderSystem.MaxLuminance = value;
        }
    }
}
