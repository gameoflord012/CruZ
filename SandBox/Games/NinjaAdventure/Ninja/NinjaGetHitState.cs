using System.Diagnostics;

using CruZ.GameEngine.GameSystem.Physic;
using CruZ.GameEngine.GameSystem.StateMachine;
using CruZ.GameEngine.Utility;

using Microsoft.Xna.Framework;

namespace NinjaAdventure.Ninja
{
    internal class NinjaHitState : BasicState<NinjaStateData>
    {
        protected override string StateEnterSoundResource => "sound\\ninja-hurt.ogg";

        protected override bool CanTransitionHere()
        {
            return StateData.MonsterCount > 0 && _hitTimer.GetElapsed() > TimeBeetweenHit;
        }

        protected override void OnStateMachineAttached()
        {
            base.OnStateMachineAttached();
            _health = StateData.Health;
            _physic = StateData.Physic;

            _hitTimer.Start();
        }

        protected override void OnStateEnter()
        {
            base.OnStateEnter();

            _stunTimer.Start();
            _stunSpeed = StunForce;
            _health.Current -= 5;
        }

        protected override void OnStateUpdate(GameTime gameTime)
        {
            base.OnStateUpdate(gameTime);

            if (_health.Current == 0)
            {
                Check(typeof(NinjaDieState));
            }
            else
            if (_stunTimer.GetElapsed() > StunTime)
            {
                Check(typeof(NinjaMovingState));
            }

            var stunDirection = _physic.Position - StateData.LastMonsterBody.Position;
            if (stunDirection.SqrMagnitude() > 0.1) stunDirection.Normalize();

            _physic.LinearVelocity = stunDirection * _stunSpeed;
            _stunSpeed *= 0.85f;
            if (_stunSpeed < 0.5) _stunSpeed = 0.5f;
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();

            _hitTimer.Restart();
            _stunTimer.Reset();

            _physic.LinearVelocity = Vector2.Zero;
        }

        const float TimeBeetweenHit = 1f;
        Stopwatch _hitTimer = new();

        const float StunTime = 0.25f;
        Stopwatch _stunTimer = new();

        const float StunForce = 10f;
        float _stunSpeed;

        HealthComponent _health;
        PhysicBodyComponent _physic;
    }
}
