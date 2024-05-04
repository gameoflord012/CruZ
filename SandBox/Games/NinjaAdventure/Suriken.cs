using System.Diagnostics;

using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.ECS;
using CruZ.GameEngine.GameSystem.Physic;
using CruZ.GameEngine.GameSystem.Render;
using CruZ.GameEngine.GameSystem.Scene;
using CruZ.GameEngine.GameSystem.Script;
using CruZ.GameEngine.Utility;

using Genbox.VelcroPhysics.Collision.ContactSystem;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NinjaAdventure
{
    internal class Suriken : IDisposable
    {
        public Suriken(GameScene gameScene, SpriteRendererComponent surikenRenderer, Vector2 origin, Vector2 direction)
        {
            _surikenRenderer = surikenRenderer;
            _surikenRenderer.DrawRequestsFetching += Renderer_DrawRequestsFetching;

            Entity = gameScene.CreateEntity();
            Entity.Name = $"Suriken {Entity.Id}";

            var script = new ScriptComponent();
            {
                script.Updating += Script_Updating;
            }
            Entity.AddComponent(script);

            _physic = new PhysicBodyComponent();
            {
                FixtureFactory.AttachCircle(0.5f, 1, _physic.Body);
                _physic.BodyType = BodyType.Dynamic;
                _physic.IsSensor = true;
                _physic.Postion = origin;
                // velocity
                if(direction.SqrMagnitude() != 0) direction.Normalize();
                _physic.LinearVelocity = direction * _moveSpeed;
                _physic.AngularVelocity = _rotationSpeed;
                _physic.UserData = this;
                // event
                _physic.OnCollision += Physic_OnCollision;
            }
            Entity.AddComponent(_physic);

            _surikenTex = GameApplication.Resource.Load<Texture2D>("art\\suriken\\01.png");
        }

        private void Script_Updating(GameTime gameTime)
        {
            _disappearTime -= gameTime.DeltaTime();
            if(_disappearTime < 0) MakeUseless();
        }

        private void Renderer_DrawRequestsFetching(List<DrawRequestBase> drawRequests)
        {
            SpriteDrawArgs drawArgs = new();
            drawArgs.Apply(Entity.Transform);
            drawArgs.Apply(_surikenTex);
            drawArgs.Scale = new Vector2(1f / _surikenTex.Width, 1f / _surikenTex.Height);

            if (!drawArgs.IsOutOfScreen(Camera.Main.ProjectionMatrix()))
            {
                drawRequests.Add(new SpriteDrawRequest(drawArgs));
            }
            else
            {
                //Debugger.Break();
                MakeUseless();
            }
        }
        private void Physic_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if(fixtureB.Body.UserData is LarvaMonster)
                MakeUseless();
        }

        private void MakeUseless()
        {
            _physic.Awake = false;
            BecomeUseless?.Invoke();
        }

        public TransformEntity Entity;

        Texture2D _surikenTex;
        SpriteRendererComponent _surikenRenderer;

        float _moveSpeed = 12f;
        float _rotationSpeed = 20f;
        float _disappearTime = 3.5f; // seconds
        private PhysicBodyComponent _physic;

        public event Action? BecomeUseless;

        public void Dispose()
        {
            _surikenRenderer.DrawRequestsFetching -= Renderer_DrawRequestsFetching;
            _physic.OnCollision -= Physic_OnCollision;
            BecomeUseless = default;
            Entity.Dispose();
        }
    }
}
