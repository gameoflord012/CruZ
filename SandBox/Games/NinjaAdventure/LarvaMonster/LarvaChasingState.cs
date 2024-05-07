using CruZ.GameEngine.GameSystem.Animation;
using CruZ.GameEngine.GameSystem.Physic;
using CruZ.GameEngine.GameSystem.StateMachine;
using CruZ.GameEngine.Utility;

using Microsoft.Xna.Framework;

namespace NinjaAdventure
{
    internal class LarvaChasingState : StateBase<LarvaStateData>
    {
        protected override void OnStateMachineAttached()
        {
            base.OnStateMachineAttached();
            _physic = StateData.Physic;
            _animation = StateData.Animation;
        }

        protected override void OnStateUpdate(GameTime gameTime)
        {
            // stay if follow target unavai
            if(StateData.Follow == null)
            {
                _physic.AngularVelocity = 0;
                _physic.LinearVelocity = Vector2.Zero;
            }
            else
            {
                _facingDir = Vector2.Rotate(Vector2.UnitY, _physic.Transform.Rotation);
                _facingDir.Normalize();

                Vector2 followDir = StateData.Follow.Position - _physic.Position;
                if (followDir.Length() > 0.01) followDir.Normalize();

                var rotationDir = MathF.Sign(FunMath.GetAngleBetween(_facingDir, followDir));

                _physic.AngularVelocity = rotationDir * _rotationSpeed;
                _physic.LinearVelocity = _facingDir * _speed;
            }

            UpdateAnimation();
        }

        protected override void OnTransitionChecking()
        {
            base.OnTransitionChecking();

            Check(typeof(LarvaHitState));
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
            _animation.Stop();
            _physic.AngularVelocity = 0;
            _physic.LinearVelocity = Vector2.Zero;
        }

        private void UpdateAnimation()
        {
            _facingString ??= "front";
            _facingString = AnimationHelper.GetFacingDirectionString(_facingDir, _facingString);
            _animation.Play($"walk-{_facingString}");
        }

        private string? _facingString;
        private Vector2 _facingDir;

        PhysicBodyComponent _physic;
        AnimationComponent _animation;

        float _speed = 1;
        float _rotationSpeed = 3.14f;
    }
}
