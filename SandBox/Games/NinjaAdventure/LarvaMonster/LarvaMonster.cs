using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.Animation;
using CruZ.GameEngine.GameSystem.ECS;
using CruZ.GameEngine.GameSystem.Physic;
using CruZ.GameEngine.GameSystem.Scene;

using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;
using Genbox.VelcroPhysics.Collision.ContactSystem;

using Microsoft.Xna.Framework;
using CruZ.GameEngine.GameSystem.StateMachine;
using System;
using System.Collections.Generic;

namespace NinjaAdventure
{
    internal class LarvaMonster : IDisposable
    {
        public event Action<LarvaMonster>? PoolReturn;

        public LarvaMonster(GameScene scene, SpriteRendererComponent spriteRenderer)
        {
            _spriteRenderer = spriteRenderer;
            Entity = scene.CreateEntity();
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
                _physic.OnCollision += Physic_OnCollision;
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
            _onPool = false;

            // reset state data
            _stateData.Reset(follow);
            _machine.SetNextState(typeof(LarvaChasingState), false);

            // reset physic
            _physic.Position = position;
            _physic.LinearVelocity = Vector2.Zero;
            _physic.Rotation = 0;
        }

        private void Physic_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.Body.UserData is Suriken suriken)
            {
                _stateData.HitBodies.Add(fixtureB.Body);
                _surikenToBody[suriken] = fixtureB.Body;
            }
        }

        public void ReturnToPool()
        {
            if(_onPool) return;
            _onPool = true;

            PoolReturn?.Invoke(this);
        }

        public TransformEntity Entity
        {
            get;
            private set;
        }

        Dictionary<Suriken, Body> _surikenToBody;

        AnimationComponent _animation;
        SpriteRendererComponent _spriteRenderer;
        PhysicBodyComponent _physic;
        HealthComponent _health;

        StateMachineComponent _machine;
        LarvaStateData _stateData;

        bool _onPool;

        public void Dispose()
        {
            Entity.Dispose();
            _physic.OnCollision -= Physic_OnCollision;
            PoolReturn = default;
        }
    }
}
