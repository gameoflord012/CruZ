using CruZ.GameEngine.GameSystem.Animation;
using CruZ.GameEngine.GameSystem.Physic;
using CruZ.GameEngine.GameSystem.StateMachine;
using CruZ.GameEngine.Input;
using CruZ.GameEngine.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace NinjaAdventure
{
    internal class NinjaMovingState : StateBase
    {
        protected override void OnStateEnter()
        {
            InputManager.KeyStateChanged += Input_KeyStateChanged;
            _physic = GetData<PhysicBodyComponent>("PhysicComponent");
            _animationComponent = GetData<AnimationComponent>("AnimationComponent");
            _ninjaInput.Movement = Vector2.Zero;
            _ninjaInput.FireSuriken = false;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);
            _physic.Position += _ninjaInput.Movement * gameTime.DeltaTime() * _speed;

            float lastAttackTime = GetData<float>("LastAttackTime");

            if (_ninjaInput.FireSuriken && _timeBetweenAttacks < gameTime.TotalGameTime() - lastAttackTime)
            {
                Machine.SetNextState(typeof(NinjaAttackState));
            }

            var facingDir = AnimationHelper.GetFacingDirectionString(_ninjaInput.Movement);
            _animationComponent.Play($"walk-{facingDir}");

            SetData("FacingDirectionString", facingDir);
            SetData("MovingDirection", _ninjaInput.Movement);
        }
        protected override void OnStateExit()
        {
            base.OnStateExit();
            _animationComponent.Stop();
            InputManager.KeyStateChanged -= Input_KeyStateChanged;
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

        float _timeBetweenAttacks = 0.4f;

        PhysicBodyComponent _physic;
        AnimationComponent _animationComponent;

        public override void Dispose()
        {
            base.Dispose();

            InputManager.KeyStateChanged -= Input_KeyStateChanged;
        }
    }
}
