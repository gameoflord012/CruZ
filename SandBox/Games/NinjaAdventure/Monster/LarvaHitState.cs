using System.Diagnostics;
using System.Linq;

using CruZ.GameEngine.GameSystem.Physic;
using CruZ.GameEngine.GameSystem.StateMachine;
using CruZ.GameEngine.Utility;

using Genbox.VelcroPhysics.Dynamics;

using Microsoft.Xna.Framework;

namespace NinjaAdventure
{
    internal class LarvaHitState : BasicState<LarvaStateData>
    {
        const float StunSpeedMultiplier = 0.9f;
        const float StunForce = 7f;
        const float StunTime = 0.35f;

        protected override string StateEnterSoundResource => "sound\\larva-hit.mp3";

        protected override bool CanTransitionHere()
        {
            return StateData.HitOrigins.Count > 0;
        }
        protected override void OnStateEnter()
        {
            base.OnStateEnter();

            _physic = StateData.Physic;
            _hitOrigin = StateData.HitOrigins.Pop();

            _stunTimer.Start();
            _stunSpeed = StunForce;

            StateData.Health.Current -= 5;
        }

        protected override void OnTransitionChecking()
        {
            base.OnTransitionChecking();

            // if Larva get hit in the middle, reset the state 
            if (StateData.HitOrigins.Count > 0)
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

        protected override void OnStateUpdate(StateUpdateArgs args)
        {
            var stunDirection = _physic.Position - _hitOrigin;
            if (stunDirection.SqrMagnitude() > 0.1) stunDirection.Normalize();

            _physic.LinearVelocity = stunDirection * _stunSpeed;
            _stunSpeed *= StunSpeedMultiplier;
            if (_stunSpeed < 0.5) _stunSpeed = 0.5f;
        }

        protected override void OnStateExit()
        {
            _physic.LinearVelocity = Vector2.Zero;
            _stunTimer.Reset();
        }

        PhysicBodyComponent _physic;
        Vector2 _hitOrigin;

        Stopwatch _stunTimer = new();
        float _stunSpeed;
    }
}
