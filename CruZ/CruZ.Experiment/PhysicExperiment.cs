using System;
using System.Diagnostics;

using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem;

using Genbox.VelcroPhysics.Collision.ContactSystem;
using Genbox.VelcroPhysics.Dynamics;
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
            Content.RootDirectory = ".\\Content";

            _world = new(-Vector2.UnitY * 10);
            _debugView = new(_world);

            _floor = BodyFactory.CreateRectangle(_world, 400, 100f, 1f, new Vector2(0, -200));
            _floor.BodyType = BodyType.Static;

            _agent = BodyFactory.CreateRectangle(_world, 100, 100f, 1f, new Vector2(0, 0));
            _agent.BodyType = BodyType.Dynamic;
            _agent.IsSensor = true;

            _agent.OnCollision = OnCollisionHandler;
        }

        public void OnCollisionHandler(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            Console.WriteLine($"{fixtureA} collide with {fixtureB}!");
        }


        protected override void LoadContent()
        {
            base.LoadContent();

            _texture = Content.Load<Texture2D>("homelander");
            _normalFx = Content.Load<Effect>("shaders\\normal-shader");
            _debugView.LoadContent(GraphicsDevice, Content);
        }

        protected override void OnInitialize()
        {
            _spriteBatch = new(GraphicsDevice);
            _camera = new(Window);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            _world.Step(gameTime.GetElapsedSeconds());

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

            if(Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                _rotation -= gameTime.GetElapsedSeconds() * 3.14f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                _rotation += gameTime.GetElapsedSeconds() * 3.14f;
            }

            _position += dir * (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
        }

        protected override void OnDraw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Pink);
            var proj = _camera.ProjectionMatrix();
            var view = _camera.ViewMatrix();

            _normalFx.Parameters["view_projection"].SetValue(view * proj);

            _spriteBatch.Begin(effect: _normalFx);
            _spriteBatch.DrawWorld(
                _texture, 
                _position,
                new Rectangle(_texture.Width / 2, _texture.Height / 2, _texture.Width / 2, _texture.Height / 2),
                Color.White,
                _rotation,
                new(0, 0),
                Vector2.One,
                SpriteEffects.None,
                0);
            _spriteBatch.End();

            _debugView.RenderDebugData(
                _camera.ViewMatrix(), 
                _camera.ProjectionMatrix());
        }

        Texture2D _texture;
        SpriteBatch _spriteBatch;
        Effect _normalFx;
        Camera _camera;

        DebugView _debugView;
        World _world;

        Body _floor, _agent;

        Vector2 _position;
        float _rotation;
    }
}
