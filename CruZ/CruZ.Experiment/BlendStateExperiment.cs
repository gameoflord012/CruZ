using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CruZ.GameEngine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.Experiment
{
    internal class BlendStateExperiment : GameWrapper
    {
        public BlendStateExperiment()
        {
            Content.RootDirectory = ".\\Content\\bin";
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _sp = new SpriteBatch(GraphicsDevice);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            _tex = Content.Load<Texture2D>("homelander");
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);  

            _sp.Begin(blendState: BlendState.Additive);
            _sp.Draw(_tex, Vector2.Zero, Color.White);
            _sp.End();

        }

        SpriteBatch _sp;
        Texture2D _tex;
    }
}
