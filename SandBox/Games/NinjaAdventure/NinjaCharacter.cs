using System.Diagnostics;

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

            _animationComponent = new AnimationComponent(spriteRenderer);
            {
                _animationComponent.FitToWorldUnit = true;
                _animationComponent.LoadAnimationFile("art\\NinjaAnim.aseprite");
            }
            Entity.AddComponent(_animationComponent);

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
            var facingString = AnimationHelper.GetFacingDirectionString(_inputMovement);

            foreach (var useless in uselessSurikens)
            {
                useless.Dispose();
            }

            uselessSurikens.Clear();

            var isAttackAnimationPlaying =
                _animationComponent.IsAnimationPlaying() &&
                _animationComponent.CurrentAnimationName().StartsWith("attack");

            //
            // movement update
            //
            if (!isAttackAnimationPlaying) // don't move when attacking
                Entity.Transform.Position += _inputMovement * gameTime.GetElapsedSeconds() * _speed;

            //
            // spawning suriken
            //

            if (_inputFireSuriken && _timeBetweenAttacks < _attackTimer)
            {
                _attackTimer = 0;

                var suriken = new Suriken(_gameScene, _surikenRenderer, Entity.Position, _inputMovement);
                suriken.BecomeUseless += () => uselessSurikens.Add(suriken);

                _animationComponent.PlayAnimation($"attack-{facingString}", 1);
                isAttackAnimationPlaying = true;
            }

            if(!isAttackAnimationPlaying) // we don't want moving animation playing when player attacking
            {
                _animationComponent.PlayAnimation($"walk-{facingString}");
            }

            if (_inputFireSuriken) _inputFireSuriken = false;
            _attackTimer += gameTime.GetElapsedSeconds();
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
            if (inputInfo.IsKeyHeldDown(Keys.W))
            {
                _inputMovement += new Vector2(0, 1);
            }
            if (inputInfo.IsKeyHeldDown(Keys.S))
            {
                _inputMovement += new Vector2(0, -1);
            }

            if (inputInfo.IsKeyJustDown(Keys.Space))
            {
                _inputFireSuriken = true;
            }
        }

        List<Suriken> uselessSurikens = [];

        Vector2 _inputMovement;

        bool _inputFireSuriken;
        float _speed = 4;

        float _attackTimer = 0f;
        float _timeBetweenAttacks = 0.4f;

        AnimationComponent _animationComponent;
        ScriptComponent _scriptComponent;
        SpriteRendererComponent _surikenRenderer;

        GameScene _gameScene;

        public TransformEntity Entity;
    }
}
