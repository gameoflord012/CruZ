using System.IO;

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.IO;

using CruZ.GameEngine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Aseprite;

namespace CruZ.Experiment
{
    internal class AsepriteExperiment : GameWrapper
    {
        public AsepriteExperiment()
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

            AsepriteFile aseFile;
            using (Stream stream = TitleContainer.OpenStream("Content\\ninja.aseprite"))
            {
                aseFile = AsepriteFileLoader.FromStream("ninja", stream, preMultiplyAlpha: true);
            }

            _spriteSheet = aseFile.CreateSpriteSheet(GraphicsDevice);
            _idle = _spriteSheet.CreateAnimatedSprite("idle");
            _idle.Play();
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            _idle.Update(gameTime);
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);  

            _sp.Begin(samplerState: SamplerState.PointClamp);
            _sp.Draw(_idle, Vector2.Zero);
            _sp.End();

        }

        SpriteBatch _sp;
        SpriteSheet _spriteSheet;

        AnimatedSprite _idle;
    }
}
