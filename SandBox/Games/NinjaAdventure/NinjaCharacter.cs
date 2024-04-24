using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.Animation;
using CruZ.GameEngine.GameSystem.ECS;
using CruZ.GameEngine.GameSystem.Scene;
using CruZ.GameEngine.GameSystem.Script;
using CruZ.GameEngine.Input;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace NinjaAdventure
{
    internal class NinjaCharacter
    {
        public NinjaCharacter(GameScene scene, SpriteRendererComponent spriteRenderer)
        {
            _gameScene = scene;
            Entity = scene.CreateEntity("Ninja");

            _animation = new AnimationComponent();
            {
                _animation.Renderer = spriteRenderer;
                _animation.FitToWorldUnit = true;
                _animation.LoadAnimationFile("art\\NinjaAnim.aseprite");
            }
            Entity.AddComponent(_animation);

            _scriptComponent = new ScriptComponent();
            {
                _scriptComponent.Updating += Script_Updating;
            }
            Entity.AddComponent(_scriptComponent);

            _surikenRenderer = new SpriteRendererComponent();
            {

            }
            Entity.AddComponent(_surikenRenderer);

            InputManager.KeyStateChanged += Input_KeyStateChanged;
        }

        private void Script_Updating(GameTime gameTime)
        {
            //
            // movement update
            //
            Entity.Transform.Position += _inputMovement * gameTime.GetElapsedSeconds() * _speed;
            if (_inputMovement.LengthSquared() > 0.01f) _facingDirection = _inputMovement;

            //
            // spawning suriken
            //
            if (_inputFireSuriken)
            {
                var suriken = new Suriken(_gameScene, _surikenRenderer, Entity.Position, _facingDirection);
                suriken.BecomeUseless += suriken.Dispose;
                _inputFireSuriken = false;
            }

            //
            // animations
            //
            string movingAnimation = GetMovingAnimationName();
            _animation.PlayAnimation(movingAnimation);
        }

        private string GetMovingAnimationName()
        {
            if (Vector2.Dot(_facingDirection, Vector2.UnitX) == 1) // facing right
            {
                return "walk-right";
            }

            if (Vector2.Dot(_facingDirection, Vector2.UnitX) == -1) // facing left
            {
                return "walk-left";
            }

            if (Vector2.Dot(_facingDirection, Vector2.UnitY) == -1) // facing back
            {
                return "walk-back";
            }

            return "walk-front";
        }

        private void Input_KeyStateChanged(IInputInfo inputInfo)
        {
            _inputMovement = Vector2.Zero;
            _inputFireSuriken = false;

            if (inputInfo.IsKeyHeldDown(Keys.A))
            {
                _inputMovement += new Vector2(-1, 0);
            }
            if (inputInfo.IsKeyHeldDown(Keys.D))
            {
                _inputMovement += new Vector2(1, 0);
            }
            if (inputInfo.IsKeyHeldDown(Keys.S))
            {
                _inputMovement += new Vector2(0, 1);
            }
            if (inputInfo.IsKeyHeldDown(Keys.W))
            {
                _inputMovement += new Vector2(0, -1);
            }

            if (inputInfo.IsKeyJustDown(Keys.Space))
            {
                _inputFireSuriken = true;
            }
        }

        List<Suriken> surikens = [];

        Vector2 _inputMovement;
        Vector2 _facingDirection;
        bool _inputFireSuriken;

        float _speed = 4;

        AnimationComponent _animation;
        ScriptComponent _scriptComponent;
        SpriteRendererComponent _surikenRenderer;

        GameScene _gameScene;

        public TransformEntity Entity;
    }
}
