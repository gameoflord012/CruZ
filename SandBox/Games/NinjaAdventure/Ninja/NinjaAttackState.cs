using System.Diagnostics;

using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem.Animation;
using CruZ.GameEngine.GameSystem.StateMachine;
using CruZ.GameEngine.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using MonoGame.Aseprite;

using NinjaAdventure.Ninja;

namespace NinjaAdventure
{
    internal class NinjaAttackState : StateBase<NinjaStateData>
    {
        protected override void OnStateMachineAttached()
        {
            _surikenThrowSoundFx = GameApplication.Resource.Load<SoundEffect>("sound\\throw-suriken.mp3");
            _animationComponent = StateData.Animation;
            _ninjaCharacter = StateData.NinjaCharacter;

            _attackTimer.Start();
        }

        protected override bool CanTransitionHere()
        {
            return _attackTimer.GetElapsed() > TimeBetweenAttacks;
        }

        protected override void OnTransitionChecking()
        {
            Check(typeof(NinjaHitState));
        }

        protected override void OnStateEnter()
        {
            _attackTimer.Restart();

            _surikenThrowSoundFx.Play();
            _animationComponent.Play($"attack-{StateData.GetFacingString()}", 1, OnAnimationEnd);
            _ninjaCharacter.SpawnSuriken(StateData.LastInputMovement);            
        }

        private void OnAnimationEnd(AnimatedSprite sprite)
        {
            Check(typeof(NinjaMovingState));
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
            _animationComponent.Stop();
        }

        NinjaCharacter _ninjaCharacter;
        SoundEffect _surikenThrowSoundFx;
        AnimationComponent _animationComponent;

        float TimeBetweenAttacks = 0.4f;
        Stopwatch _attackTimer = new();
    }
}
