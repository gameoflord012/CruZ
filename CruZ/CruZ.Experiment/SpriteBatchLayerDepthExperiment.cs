using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CruZ.Common;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.Experiment
{
    internal class SpriteBatchLayerDepthExperiment : GameWrapper
    {
        public SpriteBatchLayerDepthExperiment()
        {
            Content.RootDirectory = ".\\Content\\bin";
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _camera = new Camera(GraphicsDevice.Viewport);
            _sp = new SpriteBatch(GraphicsDevice);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _tex = Content.Load<Texture2D>("homelander");
            _normalFx = Content.Load<Effect>("shaders\\normal-shader");
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);  

            _normalFx.Parameters["view_projection"].SetValue(_camera.ViewMatrix() * _camera.ProjectionMatrix());

            _sp.Begin(effect: _normalFx);
            _sp.Draw(_tex, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 11);
            _sp.End();

        }

        SpriteBatch _sp;
        Texture2D _tex;
        Camera _camera;
        Effect _normalFx;
    }
}
