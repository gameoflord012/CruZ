using System.Diagnostics;

using CruZ.GameEngine.GameSystem.Physic;
using CruZ.GameEngine.GameSystem.StateMachine;
using CruZ.GameEngine.Utility;

using Microsoft.Xna.Framework;

namespace NinjaAdventure.Ninja
{
    internal class NinjaGetHitState : BasicState<NinjaStateData>
    {
        protected override void OnTransitionChecking()
        {
            base.OnTransitionChecking();

            if(Machine.CurrentState == typeof(NinjaDieState)) return;

            var monsterCount = StateData.MonsterCount;
            if (monsterCount > 0 && _hitTimer.GetElapsed() > TimeBeetweenHit)
            {
                Machine.SetNextState(typeof(NinjaGetHitState));
            }
        }

        protected override void OnAdded()
        {
            base.OnAdded();
            _health = StateData.Health;
            _physic = StateData.Physic;

            _hitTimer.Start();
            _stunTimer.Start();
        }

        protected override string? GetStateEnterSoundResource()
        {
            return "sound\\ninja-hurt.ogg";
        }

        protected override void OnStateEnter()
        {
            base.OnStateEnter();

            _health.Current -= 5;
            _stunSpeed = StunForce;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            if (_health.Current == 0)
                Machine.SetNextState(typeof(NinjaDieState));
            else 
            if (_stunTimer.GetElapsed() > StunTime)
            {
                Machine.SetNextState(typeof(NinjaMovingState));
            }

            var stunDirection = _physic.Position - StateData.HitMonsterPosition;
            if(stunDirection.SqrMagnitude() > 0.1) stunDirection.Normalize();

            _physic.LinearVelocity = stunDirection * _stunSpeed;
            _stunSpeed *= 0.85f;
            if (_stunSpeed < 0.5) _stunSpeed = 0.5f;
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
            _hitTimer.Restart();
            _stunTimer.Restart();
        }

        const float TimeBeetweenHit = 1f;
        Stopwatch _hitTimer = new(); 

        const float StunTime = 0.5f;
        Stopwatch _stunTimer = new();

        const float StunForce = 5f;
        float _stunSpeed;

        HealthComponent _health;
        PhysicBodyComponent _physic;
    }
}
