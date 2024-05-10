using CruZ.GameEngine.GameSystem.Animation;
using CruZ.GameEngine.GameSystem.Physic;
using CruZ.GameEngine.GameSystem.StateMachine;
using CruZ.GameEngine.Input;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using NinjaAdventure.Ninja;

namespace NinjaAdventure
{
    internal class NinjaMovingState : StateBase<NinjaStateData>
    {
        protected override void OnStateMachineAttached()
        {
            base.OnStateMachineAttached();
            UISystem.Instance.KeyStateChanged += Input_KeyStateChanged;
        }

        protected override void OnStateEnter()
        {
            _physic = StateData.Physic;
            _animationComponent = StateData.Animation;
        }

        protected override void OnStateUpdate(GameTime gameTime)
        {
            base.OnStateUpdate(gameTime);
            _physic.LinearVelocity = _ninjaInput.Movement * _speed;

            if (_ninjaInput.FireSuriken)
            {
                Check(typeof(NinjaAttackState));
            }
 
            StateData.LastInputMovement = _ninjaInput.Movement;
            _animationComponent.Play($"walk-{StateData.GetFacingString()}");
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
            _animationComponent.Stop();
            _physic.LinearVelocity = Vector2.Zero;
        }

        protected override void OnTransitionChecking()
        {
            base.OnTransitionChecking();
            Check(typeof(NinjaHitState));
        }

        private void Input_KeyStateChanged(IInputInfo inputInfo)
        {
            _ninjaInput.Movement = Vector2.Zero;
            _ninjaInput.FireSuriken = false;

            if (inputInfo.IsKeyHeldDown(Keys.A))
            {
                _ninjaInput.Movement += new Vector2(-1, 0);
            }
            if (inputInfo.IsKeyHeldDown(Keys.D))
            {
                _ninjaInput.Movement += new Vector2(1, 0);
            }
            if (inputInfo.IsKeyHeldDown(Keys.W))
            {
                _ninjaInput.Movement += new Vector2(0, 1);
            }
            if (inputInfo.IsKeyHeldDown(Keys.S))
            {
                _ninjaInput.Movement += new Vector2(0, -1);
            }

            if (inputInfo.IsKeyJustDown(Keys.Space))
            {
                _ninjaInput.FireSuriken = true;
            }
        }

        float _speed = 4;
        record struct Input(Vector2 Movement, bool FireSuriken);
        Input _ninjaInput;

        private PhysicBodyComponent _physic;
        private AnimationComponent _animationComponent;

        public override void Dispose()
        {
            base.Dispose();
            InputManager.KeyStateChanged -= Input_KeyStateChanged;
        }
    }
}
