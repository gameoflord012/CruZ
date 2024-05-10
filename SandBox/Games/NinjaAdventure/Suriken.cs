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
using Genbox.VelcroPhysics.Collision.Narrowphase;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NinjaAdventure
{
    internal class Suriken : IDisposable, IPoolObject
    {
        private const float SurikenSize = 0.6f;
        private const float MoveSpeed = 12;
        private const float RotationSpeed = 20f;
        private const float DisappearTime = 3.5f;

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

        private void Script_Updating(ScriptUpdateArgs args)
        {
            if(_disappearTimer < 0) return;

            _disappearTimer -= args.GameTime.DeltaTime();
            if(_disappearTimer < 0 || _hit) ReturnToPool();
        }

        private void Renderer_DrawRequestsFetching(List<DrawRequestBase> drawRequests)
        {
            SpriteDrawArgs drawArgs = new();
            drawArgs.Apply(Entity.Transform);
            drawArgs.Apply(_surikenTex);
            drawArgs.Scale = new Vector2(SurikenSize / _surikenTex.Width, SurikenSize / _surikenTex.Height);

            if(!drawArgs.IsOutOfScreen(Camera.Main.ProjectionMatrix()))
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
            if(fixtureB.Body.UserData is LarvaMonster)
            {
                if(!fixtureB.Body.Awake)
                {
                    contact.Enabled = false;
                }
                else
                {
                    _hit = true;
                }
            }
        }

        public void Reset(Vector2 origin, Vector2 direction)
        {
            _disappearTimer = DisappearTime;

            // Physic
            _physic.Position = origin;
            if(direction.SqrMagnitude() != 0) direction.Normalize();
            _physic.LinearVelocity = direction * MoveSpeed;
            _physic.AngularVelocity = RotationSpeed;
            _physic.Body.Awake = true;

            // events
            _physic.OnCollision += Physic_OnCollision;
            _script.Updating += Script_Updating;
            _surikenRenderer.DrawRequestsFetching += Renderer_DrawRequestsFetching;

            _hit = false;
        }

        private void ReturnToPool()
        {
            ((IPoolObject)this).Pool.ReturnPoolObject(this);
        }

        void IPoolObject.OnDisabled()
        {
            _physic.Position = Vector2.Zero;
            _physic.LinearVelocity = Vector2.Zero;
            _physic.AngularVelocity = 0;
            _physic.Body.Awake = false;

            _physic.OnCollision -= Physic_OnCollision;
            _script.Updating -= Script_Updating;
            _surikenRenderer.DrawRequestsFetching -= Renderer_DrawRequestsFetching;
        }

        Pool IPoolObject.Pool
        {
            get;
            set;
        }

        public TransformEntity Entity;

        private readonly Texture2D _surikenTex;
        private readonly SpriteRendererComponent _surikenRenderer;
        private readonly PhysicBodyComponent _physic;
        private readonly ScriptComponent _script;
        private float _disappearTimer;
        private bool _hit;

        public void Dispose()
        {
            ReturnToPool();
            _surikenRenderer.DrawRequestsFetching -= Renderer_DrawRequestsFetching;
            _physic.OnCollision -= Physic_OnCollision;
            Entity.Dispose();
        }
    }
}
