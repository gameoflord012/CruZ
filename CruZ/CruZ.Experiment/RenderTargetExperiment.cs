using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.Experiment
{
    internal class RenderTargetExperiment : GameWrapper
    {
        public RenderTargetExperiment()
        {
            Content.RootDirectory = ".\\Content\\bin\\";
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _camera = new Camera(Window);
            _sp = new SpriteBatch(GraphicsDevice);
            _renderTarget = new RenderTarget2D(
                GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, false, 
                SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
            
            GraphicsDevice.PresentationParameters.RenderTargetUsage = RenderTargetUsage.DiscardContents;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _tex = Content.Load<Texture2D>("homelander");
            _normalFx = Content.Load<Effect>("shaders\\normal-shader");
        }

        protected override void OnDrawing(GameTime gameTime)
        {
            base.OnDrawing(gameTime); 
            
            GraphicsDevice.SetRenderTarget(_renderTarget);

            _normalFx.Parameters["view_projection"].SetValue(_camera.ViewMatrix() * _camera.ProjectionMatrix());

            _sp.Begin(effect: _normalFx);
            _sp.Draw(_tex, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 11);
            _sp.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.SetRenderTarget(_renderTarget);

            GraphicsDevice.SetRenderTarget(null);
            _sp.Begin();
            _sp.Draw(_renderTarget, Vector2.Zero, Color.White);
            _sp.End();

        }

        SpriteBatch _sp;
        Texture2D _tex;
        Camera _camera;
        Effect _normalFx;
        RenderTarget2D _renderTarget;
    }
}
