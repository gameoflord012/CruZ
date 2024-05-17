using System.Diagnostics;
using System.Linq;

using CruZ.GameEngine.GameSystem.Physic;
using CruZ.GameEngine.GameSystem.StateMachine;
using CruZ.GameEngine.Utility;

using Genbox.VelcroPhysics.Dynamics;

using Microsoft.Xna.Framework;

namespace NinjaAdventure.Ninja
{
    internal class NinjaHitState : BasicState<NinjaStateData>
    {
        private const float TimeBeetweenHit = 1f;
        private const float StunForce = 10f;
        private const float StunTime = 0.25f;

        public NinjaHitState()
        {
            _hitTimer = new();
            _stunTimer = new();
        }

        protected override string StateEnterSoundResource => "sound\\ninja-hurt.ogg";

        protected override bool CanTransitionHere()
        {
            var hitBody = GetHitMonsterBody();
            if(hitBody != null && _hitTimer.GetElapsed() > TimeBeetweenHit)
            {
                _hitOrigin = hitBody.Position;
                return true;
            }
            else
            {
                return false;
            }
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

        protected override void OnStateUpdate(StateUpdateArgs args)
        {
            base.OnStateUpdate(args);

            if(_health.Current == 0)
            {
                Check(typeof(NinjaDieState));
            }
            else
            if(_stunTimer.GetElapsed() > StunTime)
            {
                Check(typeof(NinjaMovingState));
            }

            var stunDirection = _physic.Position - _hitOrigin;
            if(stunDirection.SqrMagnitude() > 0.1) stunDirection.Normalize();

            _physic.LinearVelocity = stunDirection * _stunSpeed;
            _stunSpeed *= 0.85f;
            if(_stunSpeed < 0.5)
            {
                _stunSpeed = 0.5f;
            }
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();

            _hitTimer.Restart();
            _stunTimer.Reset();

            _physic.LinearVelocity = Vector2.Zero;
        }

        private Body? GetHitMonsterBody()
        {
            return StateData.Character.GetCollidedMonsterBodies().FirstOrDefault();
        }

        private Stopwatch _hitTimer;
        private Stopwatch _stunTimer;
        private float _stunSpeed;
        private HealthComponent _health;
        private PhysicBodyComponent _physic;
        private Vector2 _hitOrigin;
    }
}
