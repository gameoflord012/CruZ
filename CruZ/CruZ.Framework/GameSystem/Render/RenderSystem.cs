using CruZ.Framework.GameSystem.ECS;
using CruZ.Framework.GameSystem.Render.Filters;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.Framework.GameSystem.Render
{
    internal class RenderSystem : EntitySystem
    {
        public override void OnInitialize()
        {
            _gd = GameApplication.GetGraphicsDevice();
            _spriteBatch = new SpriteBatch(_gd);
            _postprocessing = new PostProcessingFilter();
            UpdateRenderTargetResolution(_gd.Viewport);
            GameApplication.WindowResized += UpdateRenderTargetResolution;
        }

        private void UpdateRenderTargetResolution(Viewport vp)
        {
            _rtSprite = new RenderTarget2D(_gd, _gd.Viewport.Width, _gd.Viewport.Height, false, SurfaceFormat.Vector4, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
        }

        protected override void OnDraw(EntitySystemEventArgs args)
        {
            var rendererComponents = args.
                ActiveEntities.GetAllComponents<RendererComponent>(GetComponentMode.Inherit);

            rendererComponents.Sort();

            _gd.SetRenderTarget(_rtSprite);
            _gd.Clear(GameConstants.GAME_BACKGROUND_COLOR);

            var eventArgs = new RendererEventArgs(
                args.GameTime,
                _spriteBatch,
                Camera.Main.ViewProjectionMatrix(),
                _rtSprite);

            foreach (var renderer in rendererComponents)
            {
                _gd.SetRenderTarget(_rtSprite); // make sure the rt is unchanged 
                renderer.Render(eventArgs);
            }

            //
            // post processing
            //
            _postprocessing.MaxLuminance = PostProcessingSettings.MaxLuminance;
            _postprocessing.Brightness = PostProcessingSettings.Brightness;
            var tonemapFilter = _postprocessing.GetFilter(_rtSprite);
            //var debug = TextureHelper.GetTextureData<Vector4>(_rtSprite);
            //debug = TextureHelper.GetTextureData<Vector4>(tonemapFilter);

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
        PostProcessingFilter _postprocessing;
    }

    public static class PostProcessingSettings
    {
        public static float MaxLuminance
        {
            get;
            set;
        } = 4f;

        public static float Brightness
        {
            get;
            set;
        } = 2f;
    }
}
