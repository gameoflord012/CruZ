using System.Diagnostics;

using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.Animation;
using CruZ.GameEngine.GameSystem.ECS;
using CruZ.GameEngine.GameSystem.Physic;
using CruZ.GameEngine.GameSystem.Scene;
using CruZ.GameEngine.GameSystem.Script;
using CruZ.GameEngine.Input;

using Genbox.VelcroPhysics.Collision.ContactSystem;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace NinjaAdventure
{
    internal class NinjaCharacter : IDisposable
    {
        public NinjaCharacter(GameScene scene, SpriteRendererComponent spriteRenderer)
        {
            _gameScene = scene;
            Entity = scene.CreateEntity("Ninja");

            _surikenThrowSoundFx = GameApplication.Resource.Load<SoundEffect>("sound\\throw-suriken.mp3");

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

            _physic = new PhysicBodyComponent();
            {
                FixtureFactory.AttachCircle(0.5f, 1, _physic.Body);
                _physic.BodyType = BodyType.Dynamic;
                _physic.IsSensor = true;
                _physic.OnCollision += Physic_OnCollision;
                _physic.OnSeperation += Physic_OnSeperation; ;
            }
            Entity.AddComponent(_physic);

            _health = new HealthComponent(30, spriteRenderer);
            {

            }
            Entity.AddComponent(_health);

            InputManager.KeyStateChanged += Input_KeyStateChanged;
        }

        private void Physic_OnSeperation(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if(IsMonster(fixtureB))
            {
                _monsterCount--;
            }
        }

        private void Physic_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (IsMonster(fixtureB))
            {
                _monsterCount++;
            }
        }

        private static bool IsMonster(Fixture fixtureB)
        {
            return fixtureB.Body.UserData is LarvaMonster;
        }

        private void Script_Updating(GameTime gameTime)
        {
            ClearUselessSuriken();

            UpdateMonsterAttacks();

            _isAttackAnimationPlaying =
                _animationComponent.IsAnimationPlaying() &&
                _animationComponent.CurrentAnimationName().StartsWith("attack");

            // movement update
            if (!_isAttackAnimationPlaying) // don't move when attacking
                _physic.Position += _ninjaInput.Movement * gameTime.GetElapsedSeconds() * _speed;

            // check can firing new suriken
            if (_ninjaInput.FireSuriken && _timeBetweenAttacks < _attackTimer)
            {
                OnStartFireSuriken();
            }

            if (!_isAttackAnimationPlaying) // we don't want moving animation playing when player attacking
            {
                _animationComponent.PlayAnimation($"walk-{CurrentFacingDir()}");
            }

            _attackTimer += gameTime.GetElapsedSeconds();
        }

        private void UpdateMonsterAttacks()
        {
        }

        private string CurrentFacingDir()
        {
            return AnimationHelper.GetFacingDirectionString(_ninjaInput.Movement);
        }

        private void ClearUselessSuriken()
        {
            foreach (var useless in uselessSurikens)
            {
                useless.Dispose();
            }

            uselessSurikens.Clear();
        }

        private void OnStartFireSuriken()
        {
            _surikenThrowSoundFx.Play();
            _attackTimer = 0;

            var suriken = new Suriken(_gameScene, _surikenRenderer, Entity.Position, _ninjaInput.Movement);
            suriken.BecomeUseless += () => uselessSurikens.Add(suriken);

            _animationComponent.PlayAnimation($"attack-{CurrentFacingDir()}", 1);
            _isAttackAnimationPlaying = true;
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

        PhysicBodyComponent _physic;
        ScriptComponent _scriptComponent;
        HealthComponent _health;
        
        AnimationComponent _animationComponent;
        bool _isAttackAnimationPlaying;

        int _monsterCount = 0;

        record struct Input(Vector2 Movement, bool FireSuriken);
        Input _ninjaInput;

        float _speed = 4;

        float _attackTimer = 0f;
        float _timeBetweenAttacks = 0.4f;

        SoundEffect _surikenThrowSoundFx;
        SpriteRendererComponent _surikenRenderer;
        List<Suriken> uselessSurikens = [];

        GameScene _gameScene;

        public TransformEntity Entity;

        public void Dispose()
        {
            _physic.OnCollision -= Physic_OnCollision;
            InputManager.KeyStateChanged -= Input_KeyStateChanged;
            _scriptComponent.Updating -= Script_Updating;

            foreach (var uselessSuriken in uselessSurikens)
            {
                uselessSuriken.Dispose();
            }
        }
    }
}
