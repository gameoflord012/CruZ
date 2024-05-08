using System;
using System.Collections.Generic;

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
    internal class Suriken : IDisposable, IPoolObject
    {
        const float SurikenSize = 0.6f;
        const float MoveSpeed = 12;
        const float RotationSpeed = 20f;
        const float DisappearTime = 3.5f;

        public Suriken(GameScene gameScene, SpriteRendererComponent surikenRenderer, TransformEntity parent)
        {
            _surikenRenderer = surikenRenderer;

            Entity = gameScene.CreateEntity();
            Entity.Name = $"Suriken {Entity.Id}";
            Entity.Parent = parent;

            _script = new ScriptComponent();
            {

            }
            Entity.AddComponent(_script);

            _physic = new PhysicBodyComponent();
            {
                FixtureFactory.AttachCircle(SurikenSize / 2f, 1, _physic.Body);
                _physic.BodyType = BodyType.Dynamic;
                _physic.IsSensor = true;
                _physic.UserData = this;
            }
            Entity.AddComponent(_physic);

            _surikenTex = GameApplication.Resource.Load<Texture2D>("art\\suriken\\01.png", true);
        }

        private void Script_Updating(GameTime gameTime)
        {
            if(_disappearTimer < 0) return;

            _disappearTimer -= gameTime.DeltaTime();
            if (_disappearTimer < 0) ReturnToPool();
        }

        private void Renderer_DrawRequestsFetching(List<DrawRequestBase> drawRequests)
        {
            SpriteDrawArgs drawArgs = new();
            drawArgs.Apply(Entity.Transform);
            drawArgs.Apply(_surikenTex);
            drawArgs.Scale = new Vector2(SurikenSize / _surikenTex.Width, SurikenSize / _surikenTex.Height);

            if (!drawArgs.IsOutOfScreen(Camera.Main.ProjectionMatrix()))
            {
                drawRequests.Add(new SpriteDrawRequest(drawArgs));
            }
            else
            {
                ReturnToPool();
            }
        }

        private void Physic_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.Body.UserData is LarvaMonster) ReturnToPool();
        }

        public void Reset(Vector2 origin, Vector2 direction)
        {
            _disappearTimer = DisappearTime;

            // Physic
            _physic.Position = origin;
            if (direction.SqrMagnitude() != 0) direction.Normalize();
            _physic.LinearVelocity = direction * MoveSpeed;
            _physic.AngularVelocity = RotationSpeed;
            _physic.Body.Awake = true;

            // events
            _physic.OnCollision += Physic_OnCollision;
            _script.Updating += Script_Updating;
            _surikenRenderer.DrawRequestsFetching += Renderer_DrawRequestsFetching;
            
        }

        Pool IPoolObject.Pool
        {
            get;
            set;
        }

        void IPoolObject.OnReturnToPool()
        {
            _physic.Position = Vector2.Zero;
            _physic.LinearVelocity = Vector2.Zero;
            _physic.AngularVelocity = 0;
            _physic.Body.Awake = false;

            _physic.OnCollision -= Physic_OnCollision;
            _script.Updating -= Script_Updating;
            _surikenRenderer.DrawRequestsFetching -= Renderer_DrawRequestsFetching;
        }

        private void ReturnToPool()
        {
            ((IPoolObject)this).Pool.ReturnPoolObject(this);
        }

        public TransformEntity Entity;
        
        Texture2D _surikenTex;
        SpriteRendererComponent _surikenRenderer;
        PhysicBodyComponent _physic;
        ScriptComponent _script;

        float _disappearTimer;

        public void Dispose()
        {
            ReturnToPool();
            _surikenRenderer.DrawRequestsFetching -= Renderer_DrawRequestsFetching;
            _physic.OnCollision -= Physic_OnCollision;
            Entity.Dispose();
        }
    }
}
