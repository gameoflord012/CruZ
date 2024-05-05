using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem.Animation;
using CruZ.GameEngine.GameSystem.ECS;
using CruZ.GameEngine.GameSystem.Scene;
using CruZ.GameEngine.GameSystem.StateMachine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using MonoGame.Aseprite;

namespace NinjaAdventure
{
    internal class NinjaAttackState : StateBase
    {
        protected override void OnAdded()
        {
            _surikenThrowSoundFx = GameApplication.Resource.Load<SoundEffect>("sound\\throw-suriken.mp3");
            SetData("LastAttackTime", float.NegativeInfinity);
        }

        protected override void OnStateEnter()
        {
            _animationComponent = GetData<AnimationComponent>("AnimationComponent");
            _ninjaCharacter = GetData<NinjaCharacter>("NinjaCharacter");
            var facingDir = GetData<string>("FacingDirectionString");

            _surikenThrowSoundFx.Play();
            _animationComponent.Play($"attack-{facingDir}", 1, OnAnimationEnd);
            _ninjaCharacter.SpawnSuriken(GetData<Vector2>("MovingDirection"));
            
            SetData("LastAttackTime", GetData<float>("TotalGameTime"));
        }

        private void OnAnimationEnd(AnimatedSprite sprite)
        {
            Machine.SetNextState(typeof(NinjaMovingState));
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
            _animationComponent.Stop();
        }

        NinjaCharacter _ninjaCharacter;
        SoundEffect _surikenThrowSoundFx;
        AnimationComponent _animationComponent;
    }
}
