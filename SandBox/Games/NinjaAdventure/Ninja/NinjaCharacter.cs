using System;

using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.Animation;
using CruZ.GameEngine.GameSystem.ECS;
using CruZ.GameEngine.GameSystem.Physic;
using CruZ.GameEngine.GameSystem.Scene;
using CruZ.GameEngine.GameSystem.StateMachine;

using Genbox.VelcroPhysics.Collision.ContactSystem;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;

using Microsoft.Xna.Framework;

using NinjaAdventure.Ninja;

namespace NinjaAdventure
{
    internal class NinjaCharacter : IDisposable
    {
        public NinjaCharacter(GameScene scene)
        {
            _gameScene = scene;
            Entity = scene.CreateEntity("Ninja");

            InitializeComponents();

            _surikenPool = new(() => new Suriken(_gameScene, _spriteRenderer!, Entity));
        }

        private void InitializeComponents()
        {
            _spriteRenderer = new SpriteRendererComponent();
            {

            }
            Entity.AddComponent(_spriteRenderer);

            _animationComponent = new AnimationComponent(_spriteRenderer);
            {
                _animationComponent.FitToWorldUnit = true;
                _animationComponent.LoadAnimationFile("anim\\Ninja\\NinjaAnim.aseprite");
            }
            Entity.AddComponent(_animationComponent);

            _physic = new PhysicBodyComponent();
            {
                FixtureFactory.AttachCircle(0.4f, 1, _physic.Body);
                _physic.BodyType = BodyType.Dynamic;
                _physic.IsSensor = true;
                _physic.OnCollision += Physic_OnCollision;
                _physic.OnSeperation += Physic_OnSeperation;
            }
            Entity.AddComponent(_physic);

            _health = new HealthComponent(30, _spriteRenderer);
            {

            }
            Entity.AddComponent(_health);

            _machine = new StateMachineComponent();
            {
                _stateData = new NinjaStateData();
                _stateData.Physic = _physic;
                _stateData.Animation = _animationComponent;
                _stateData.Health = _health;
                _stateData.SpriteRenderer = _spriteRenderer;
                _stateData.NinjaCharacter = this;

                _machine.InjectedStateData = _stateData;
                _machine.Add(new NinjaAttackState());
                _machine.Add(new NinjaMovingState());
                _machine.Add(new NinjaHitState());
                _machine.Add(new NinjaDieState());
            }
            Entity.AddComponent(_machine);

            _machine.SetNextState(typeof(NinjaMovingState), false);
        }

        internal void SpawnSuriken(Vector2 direction)
        {
            var suriken = _surikenPool.Pop();
            suriken.Reset(Entity.Position, direction);
        }

        private void Physic_OnSeperation(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if(IsMonster(fixtureB))
            {
                _stateData.MonsterCount--;
            }
        }

        private void Physic_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if(IsMonster(fixtureB))
            {
                _stateData.MonsterCount++;
                _stateData.LastMonsterBody = fixtureB.Body;
            }
        }

        private static bool IsMonster(Fixture fixtureB)
        {
            return fixtureB.Body.UserData is LarvaMonster;
        }

        public TransformEntity Entity
        {
            get;
            private set;
        }

        private PhysicBodyComponent _physic;
        private HealthComponent _health;
        private StateMachineComponent _machine;
        private SpriteRendererComponent _spriteRenderer;
        private AnimationComponent _animationComponent;
        private Pool<Suriken> _surikenPool;
        private GameScene _gameScene;
        private NinjaStateData _stateData;

        public void Dispose()
        {
            _physic.OnCollision -= Physic_OnCollision;
        }
    }
}
