using CruZ.GameEngine.GameSystem.StateMachine;
using CruZ.GameEngine.Utility;

using Microsoft.Xna.Framework;

namespace NinjaAdventure
{
    internal class LarvaDieState : BasicState<LarvaStateData>
    {
        private const float FadeDuration = 1f;

        protected override string StateEnterSoundResource => "sound\\larva-die.ogg";

        protected override void OnStateEnter()
        {
            base.OnStateEnter();

            _fadeCountdown = FadeDuration;
            StateData.Physic.Body.Awake = false;
            StateData.Health.ShouldDisplay = false;
        }

        protected override void OnStateUpdate(GameTime gameTime)
        {
            base.OnStateUpdate(gameTime);

            if(_fadeCountdown < 0)
            {
                StateData.Larva.ReturnToPool();
                return;
            }

            _fadeCountdown -= gameTime.DeltaTime();
            StateData.Animation.Color = new Color(1.0f, 1.0f, 1.0f, _fadeCountdown / FadeDuration);
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();

            StateData.Animation.Color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }

        private float _fadeCountdown;
    }
}
