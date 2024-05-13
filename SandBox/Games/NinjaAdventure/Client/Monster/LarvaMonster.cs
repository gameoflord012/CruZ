using System;
using System.Collections.Generic;

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

namespace NinjaAdventure
{
    internal class LarvaMonster : IDisposable, IPoolObject
    {
        private const int InitialHealth = 30;

        public LarvaMonster(GameScene scene, SpriteRendererComponent spriteRenderer)
        {
            Entity = scene.CreateEntity();

            _spriteRenderer = spriteRenderer;
            _surikenToBody = [];

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            _animation = new AnimationComponent(_spriteRenderer);
            {
                _animation.FitToWorldUnit = true;
                _animation.Scale = new Vector2(0.7f, 0.7f);
                _animation.LoadAnimationFile("anim\\Larva\\Larva.aseprite");
            }
            Entity.AddComponent(_animation);

            _physic = new PhysicBodyComponent();
            {
                FixtureFactory.AttachCircle(0.3f, 1, _physic.Body);
                _physic.UserData = this;
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
                _stateData = new(_physic, _health, _animation, this);
                _machine.InjectedStateData = _stateData;
                _machine.Add(new LarvaHitState());
                _machine.Add(new LarvaDieState());
                _machine.Add(new LarvaChasingState());
            }
            Entity.AddComponent(_machine);
        }

        public void Reset(Vector2 position, Transform? follow)
        {
            _health.Current = InitialHealth;
            _health.ShouldDisplay = true;

            // reset state data
            _stateData.Reset(follow);
            _machine.SetNextState(typeof(LarvaChasingState), false);

            // reset physic
            _physic.Position = position;
            _physic.LinearVelocity = Vector2.Zero;
            _physic.Rotation = 0;
            _physic.Body.Awake = true;

            // event
            _physic.OnCollision += Physic_OnCollision;
        }

        private void Physic_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if(fixtureB.Body.UserData is Suriken suriken)
            {
                if(fixtureB.Body.Awake)
                {
                    _stateData.HitOrigins.Push(fixtureB.Body.Position);
                    _surikenToBody[suriken] = fixtureB.Body;
                }
                else
                {
                    contact.Enabled = false;
                }
            }
        }

        public void ReturnToPool()
        {
            ((IPoolObject)this).Pool.ReturnPoolObject(this);
        }

        public TransformEntity Entity
        {
            get;
            private set;
        }

        Pool IPoolObject.Pool
        {
            get;
            set;
        }

        private readonly Dictionary<Suriken, Body> _surikenToBody;
        private AnimationComponent _animation;
        private readonly SpriteRendererComponent _spriteRenderer;
        private PhysicBodyComponent _physic;
        private HealthComponent _health;
        private StateMachineComponent _machine;
        private LarvaStateData _stateData;

        public void Dispose()
        {
            Entity.Dispose();
            _physic.OnCollision -= Physic_OnCollision;
        }

        void IPoolObject.OnDisabled()
        {
            _physic.OnCollision -= Physic_OnCollision;
            _physic.Body.Awake = false;

            _health.ShouldDisplay = false;
        }
    }
}
