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

            _soundEffect = GameApplication.Resource.Load<SoundEffect>("larva-hit.mp3");
        }

        protected override void OnUpdated(GameTime gameTime)
        {
            base.OnUpdated(gameTime);

            _soundEffect.Play();
        }

        SoundEffect _soundEffect;
    }
}
