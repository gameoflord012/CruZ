using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem.Render.Filters;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.GameEngine.GameSystem.Render
{
    internal class RenderSystem : EntitySystem
    {
        public override void OnInitialize()
        {
            _gd = GameApplication.GetGraphicsDevice();
            _spriteBatch = new SpriteBatch(_gd);
            _postprocessing = new PostProcessingFilter();
        }

        protected override void OnDraw(SystemEventArgs args)
        {
            var rendererComponents = args.
                ActiveEntities.GetAllComponents<RendererComponent>(GetComponentMode.Inherit);

            // sort by sorting layeyrs, lowest SortingLayer value get processed first
            rendererComponents.Sort();

            var rendererRT = RenderTargetSystem.RendererRT;

            _gd.SetRenderTarget(rendererRT);
            _gd.Clear(Color.Transparent);

            var eventArgs = new RenderSystemEventArgs(
                args.GameTime,
                _spriteBatch,
                Camera.Main.ViewProjectionMatrix(),
                rendererRT);

            foreach (var renderer in rendererComponents)
            {
                _gd.SetRenderTarget(rendererRT); // make sure the rt is unchanged 
                renderer.Render(eventArgs);
            }
            //
            // post processing
            //
            _postprocessing.MaxLuminance = PostProcessingSettings.MaxLuminance;
            _postprocessing.Brightness = PostProcessingSettings.Brightness;

            var tonemapFilter = _postprocessing.GetFilter(rendererRT);
            
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            _spriteBatch.Draw(tonemapFilter, Vector2.Zero, Color.White);
            _spriteBatch.End();

            _gd.SetRenderTarget(null);
        }

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
