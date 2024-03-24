using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CruZ.Framework;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.Experiment.BloomFilter
{
    internal class BloomFilterExperiment : GameWrapper
    {
        public BloomFilterExperiment()
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

            _filter = new BloomFilter(GraphicsDevice);
            _filter.LoadContent(Content);
            _tex = Content.Load<Texture2D>("homelander");
            _normalFx = Content.Load<Effect>("shaders\\normal-shader");

        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime); 
            
            GraphicsDevice.SetRenderTarget(_renderTarget);

            _filter.SetTexture(_tex);
            var _filtered = _filter.GetFiltered();

            GraphicsDevice.SetRenderTarget(null);
            _sp.Begin();
            _sp.Draw(_filtered, Vector2.Zero, Color.White);
            _sp.End();

        }

        SpriteBatch _sp;
        Texture2D _tex;
        Camera _camera;
        Effect _normalFx;
        RenderTarget2D _renderTarget;
        BloomFilter _filter;
    }
}
