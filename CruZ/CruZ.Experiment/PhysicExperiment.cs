using System;
using System.Diagnostics;
using System.Reflection.Metadata;

using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.Utility;

using Genbox.VelcroPhysics;
using Genbox.VelcroPhysics.Collision.ContactSystem;
using Genbox.VelcroPhysics.Collision.Handlers;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Extensions.DebugView;
using Genbox.VelcroPhysics.Factories;
using Genbox.VelcroPhysics.MonoGame.DebugView;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace CruZ.Experiment
{
    class PhysicExperiment : GameWrapper
    {
        public PhysicExperiment() : base()
        {
            IsMouseVisible = true;
            Window.AllowUserResizing = true;

            _world = new(-Vector2.UnitY * 10);
            _debugView = new(_world);

            _floor = BodyFactory.CreateRectangle(_world, 400, 100f, 1f, new Vector2(0, -200));
            _floor.BodyType = BodyType.Static;

            _agent = BodyFactory.CreateRectangle(_world, 100, 100f, 1f, new Vector2(0, 0));
            _agent.BodyType = BodyType.Dynamic;
            _agent.IsSensor = true;

            _agent.OnCollision = OnCollisionHandler;
            _agent.OnSeparation = OnSeparationHandler;
        }

        public void OnCollisionHandler(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            Console.WriteLine($"{fixtureA} collide with {fixtureB}!");
            _isCollision = true;
        }

        bool _isCollision;

        public void OnSeparationHandler(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            Console.WriteLine($"{fixtureA} seperate with {fixtureB}!");
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _debugView.LoadContent(GraphicsDevice, Content, "Resource\\Content");
        }

        protected override void OnInitialize()
        {
            _camera = new(Window);
        }

        protected override void OnUpdated(GameTime gameTime)
        {
            if (_isCollision)
            {
                _floor.Awake = false;
            }

            _world.Step(gameTime.DeltaTime());

            Vector2 dir = new(0, 0);

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                dir = new(-1, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                dir = new(1, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                dir = new(0, 1);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                dir = new(0, -1);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                _rotation -= gameTime.DeltaTime() * 3.14f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                _rotation += gameTime.DeltaTime() * 3.14f;
            }

            _position += dir * (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
        }

        protected override void OnDrawing(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Pink);

            _debugView.Flags |= DebugViewFlags.AABB;

            _debugView.RenderDebugData(
                _camera.ViewMatrix(),
                _camera.ProjectionMatrix());
        }

        Camera _camera;

        DebugView _debugView;
        World _world;

        Body? _floor;
        Body _agent;

        Vector2 _position;
        float _rotation;
    }
}
