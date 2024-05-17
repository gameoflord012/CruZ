using System;
using System.Collections.Generic;
using System.Linq;

using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.Animation;
using CruZ.GameEngine.GameSystem.ECS;
using CruZ.GameEngine.GameSystem.Physic;
using CruZ.GameEngine.GameSystem.Scene;
using CruZ.GameEngine.GameSystem.Script;
using CruZ.GameEngine.GameSystem.StateMachine;

using Genbox.VelcroPhysics.Collision.ContactSystem;
using Genbox.VelcroPhysics.Collision.Narrowphase;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Extensions.PhysicsLogics.PhysicsLogicBase;
using Genbox.VelcroPhysics.Factories;

using Microsoft.Xna.Framework;

using NinjaAdventure.Ninja;

namespace NinjaAdventure
{
    internal class NinjaCharacter : ScriptingEntity, IDisposable, IPoolObject
    {
        private const int InitialHealthPoint = 30;

        public NinjaCharacter(GameScene scene) : base(scene)
        {
            Entity.Name = "Ninja";
            Id = s_characterId++;

            _collidedMonsterBodies = [];
            _gameScene = scene;
            _camera = GameApplication.MainCamera;
            _surikenPool = new(() => new Suriken(_gameScene, _spriteRenderer!, Entity));

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            _healthBar = new HealthBar(_gameScene);
            {

            }
            _healthBar.ParentEntity = Entity;

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
                _stateData.Character = this;

                _machine.InjectedStateData = _stateData;
                _machine.Add(new NinjaAttackState());
                _machine.Add(new NinjaMovingState());
                _machine.Add(new NinjaHitState());
                _machine.Add(new NinjaDieState());
            }
            Entity.AddComponent(_machine);
        }

        public void Reset(Vector2 position)
        {
            _physic.OnCollision += Physic_OnCollision;
            _physic.OnSeperation += Physic_OnSeperation;
            _physic.Awake = true;
            _physic.Position = position;

            _health.Current = InitialHealthPoint;
            _health.ShouldDisplay = true;

            _stateData.Reset();
            _collidedMonsterBodies.Clear();
            _machine.SetNextState(typeof(NinjaMovingState), false);
        }

        public void SpawnSuriken(Vector2 direction)
        {
            var suriken = _surikenPool.Pop();
            suriken.Reset(Entity.Position, direction);
        }

        public IReadOnlyCollection<Body> GetCollidedMonsterBodies()
        {
            _collidedMonsterBodies.RemoveAll(e => !e.Awake);
            return _collidedMonsterBodies;
        }

        public void ReturnToPool()
        {
            ((IPoolObject)this).Pool.ReturnPoolObject(this);
        }

        protected override void OnUpdating(ScriptUpdateArgs args)
        {
            base.OnUpdating(args);

            _healthBar.Entity.Position = Entity.Position;

            _camera.Zoom = 60f;
            _camera.CameraOffset = _physic.Position;
        }

        private void Physic_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if(IsMonster(fixtureB))
            {
                _collidedMonsterBodies.Add(fixtureB.Body);
            }
        }

        private void Physic_OnSeperation(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if(IsMonster(fixtureB))
            {
                _collidedMonsterBodies.Remove(fixtureB.Body);
            }
        }

        private bool IsMonster(Fixture fixtureB)
        {
            return fixtureB.Body.UserData is LarvaMonster;
        }

        public int Id
        {
            get;
            set;
        }

        public Vector2 Position
        {
            get => _physic.Position;
        }

        Pool IPoolObject.Pool
        {
            get;
            set;
        }

        private static int s_characterId;
        private PhysicBodyComponent _physic;
        private HealthComponent _health;
        private StateMachineComponent _machine;
        private SpriteRendererComponent _spriteRenderer;
        private AnimationComponent _animationComponent;
        private Pool<Suriken> _surikenPool;
        private GameScene _gameScene;
        private NinjaStateData _stateData;
        private Camera _camera;
        private HealthBar _healthBar;
        private List<Body> _collidedMonsterBodies;

        public override void Dispose()
        {
            base.Dispose();

            _physic.OnCollision -= Physic_OnCollision;
            _physic.OnSeperation -= Physic_OnSeperation;
        }

        void IPoolObject.OnDisabled()
        {
            _physic.OnCollision -= Physic_OnCollision;
            _physic.OnSeperation -= Physic_OnSeperation;

            _physic.Awake = false;
            _health.ShouldDisplay = false;

            _animationComponent.Stop();
            _machine.SetNextState(typeof(NinjaMovingState), false);
        }
    }
}
