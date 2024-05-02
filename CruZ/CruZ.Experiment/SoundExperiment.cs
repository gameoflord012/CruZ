using CruZ.GameEngine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace CruZ.Experiment
{
    class SoundExperiment : GameWrapper
    {
        public SoundExperiment() : base()
        {
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _soundEffect = GameApplication.GameResource.Load<SoundEffect>("larva-hit.mp3");
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            _soundEffect.Play();
        }

        SoundEffect _soundEffect;
    }
}
