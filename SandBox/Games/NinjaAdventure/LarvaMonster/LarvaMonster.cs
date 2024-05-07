using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.Animation;
using CruZ.GameEngine.GameSystem.ECS;
using CruZ.GameEngine.GameSystem.Physic;
using CruZ.GameEngine.GameSystem.Scene;
using CruZ.GameEngine.GameSystem.Script;
using CruZ.GameEngine.Utility;

using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;
using Genbox.VelcroPhysics.Collision.ContactSystem;

using Microsoft.Xna.Framework;
using System.Diagnostics;
using CruZ.GameEngine.GameSystem.Render;
using MonoGame.Extended.BitmapFonts;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using CruZ.GameEngine.GameSystem.StateMachine;

namespace NinjaAdventure
{
    internal class LarvaMonster : IDisposable
    {
        public LarvaMonster(GameScene scene, SpriteRendererComponent spriteRenderer)
        {
            _spriteRenderer = spriteRenderer;
            

            Entity = scene.CreateEntity();

            _animation = new AnimationComponent(spriteRenderer);
            {
                _animation.FitToWorldUnit = true;
                _animation.Scale = new Vector2(0.7f, 0.7f);
                _animation.LoadAnimationFile("anim\\Larva\\Larva.aseprite");
            }
            Entity.AddComponent(_animation);

            var scriptComponent = new ScriptComponent();
            {
                scriptComponent.Updating += ScriptComponent_Updating;
            }
            Entity.AddComponent(scriptComponent);

            _physic = new PhysicBodyComponent();
            {
                FixtureFactory.AttachCircle(0.3f, 1, _physic.Body);
                _physic.UserData = this;
                _physic.BodyType = BodyType.Dynamic;
                _physic.IsSensor = true;
                _physic.OnCollision += Physic_OnCollision;
            }
            Entity.AddComponent(_physic);

            _health = new HealthComponent(30, spriteRenderer);
            {

            }
            Entity.AddComponent(_health);

            _machine = new StateMachineComponent();
            {
                _stateData = new LarvaStateData();
                _stateData.Physic = _physic;
                _stateData.Health = _health;
                _stateData.Animation = _animation;

                _machine.InjectedStateData = _stateData;
                _machine.Add(new LarvaHitState());
                _machine.Add(new LarvaDieState());
                _machine.Add(new LarvaChasingState());
                _machine.SetNextState(typeof(LarvaChasingState), false);
            }
            Entity.AddComponent(_machine);
        }

        private void ScriptComponent_Updating(GameTime gameTime)
        {
            if(_stateData.IsUseless) BecomeUseless?.Invoke(this);
        }

        private void Physic_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.Body.UserData is Suriken suriken)
            {
                _stateData.HitBodies.Add(fixtureB.Body);
                _surikenToBody[suriken] = fixtureB.Body;
                //suriken.BecomeUseless += Suriken_BecomeUseless;
            }
        }

        //private void Suriken_BecomeUseless(Suriken suriken)
        //{
        //    _stateData.HitBodies.Remove(_surikenToBody[suriken]);
        //    _surikenToBody.Remove(suriken);
        //}

        Dictionary<Suriken, Body> _surikenToBody = [];

        public event Action<LarvaMonster>? BecomeUseless;

        public TransformEntity Entity { get; private set; }

        public Transform? Follow 
        { 
            get => _stateData.Follow; 
            set => _stateData.Follow = value;
        }

        AnimationComponent _animation;
        SpriteRendererComponent _spriteRenderer;
        PhysicBodyComponent _physic;
        HealthComponent _health;
        
        private StateMachineComponent _machine;
        private LarvaStateData _stateData;

        public void Dispose()
        {
            Entity.Dispose();

            _physic.OnCollision -= Physic_OnCollision;

            BecomeUseless = default;
        }
    }
}
