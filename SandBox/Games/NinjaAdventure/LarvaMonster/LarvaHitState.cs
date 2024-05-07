using System.Diagnostics;

using CruZ.GameEngine.GameSystem.Physic;
using CruZ.GameEngine.GameSystem.StateMachine;
using CruZ.GameEngine.Utility;

using Genbox.VelcroPhysics.Dynamics;

using Microsoft.Xna.Framework;

namespace NinjaAdventure
{
    internal class LarvaHitState : BasicState<LarvaStateData>
    {
        protected float StunForce = 10f;
        protected float StunTime = 1f;

        protected override string? StateEnterSoundResource => "sound\\larva-hit.mp3";

        protected override bool CanTransitionTo()
        {
            return StateData.HitBodies.Count > 0;
        }
        protected override void OnStateEnter()
        {
            base.OnStateEnter();

            _physic = StateData.Physic;
            _hitBody = StateData.HitBodies.Last();
            StateData.HitBodies.Remove(_hitBody);

            _stunTimer.Start();
            _stunSpeed = StunForce;

            StateData.Health.Current -= 5;
        }

        protected override void OnTransitionChecking()
        {
            base.OnTransitionChecking();

            // if Larva get hit in the middle, reset the state 
            if (StateData.HitBodies.Count > 0)
            {
                Machine.SetNextState(typeof(LarvaHitState), false);
            }

            if (StateData.Health.Current == 0)
            {
                Check(typeof(LarvaDieState));
                return;
            }
            else
            if (_stunTimer.GetElapsed() > StunTime)
            {
                Check(typeof(LarvaChasingState));
            }
        }

        protected override void OnStateUpdate(GameTime gameTime)
        {
            base.OnStateUpdate(gameTime);

            var stunDirection = _physic.Position - _hitBody.Position;
            if (stunDirection.SqrMagnitude() > 0.1) stunDirection.Normalize();

            _physic.LinearVelocity = stunDirection * _stunSpeed;
            _stunSpeed *= 0.85f;
            if (_stunSpeed < 0.5) _stunSpeed = 0.5f;
        }

        protected override void OnStateExit()
        {
            _physic.LinearVelocity = Vector2.Zero;
            _stunTimer.Reset();
        }

        PhysicBodyComponent _physic;
        Body _hitBody;

        Stopwatch _stunTimer = new();
        float _stunSpeed;
    }
}
