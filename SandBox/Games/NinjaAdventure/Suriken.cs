using System.Diagnostics;

using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.ECS;
using CruZ.GameEngine.GameSystem.Physic;
using CruZ.GameEngine.GameSystem.Render;
using CruZ.GameEngine.GameSystem.Scene;
using CruZ.GameEngine.GameSystem.Script;
using CruZ.GameEngine.Utility;

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

            var physic = new PhysicBodyComponent();
            {
                FixtureFactory.AttachCircle(0.5f, 1, physic.Body);
                physic.Body.IsSensor = true;
                physic.Postion = origin;
                if(direction.SqrMagnitude() != 0) direction.Normalize();
                physic.LinearVelocity = direction * _moveSpeed;
                physic.AngularVelocity = _rotationSpeed;
            }
            Entity.AddComponent(physic);

            _surikenTex = GameContext.GameResource.Load<Texture2D>("art\\suriken\\01.png");
        }

        private void Script_Updating(GameTime gameTime)
        {
            _disappearTime -= gameTime.GetElapsedSeconds();
            if(_disappearTime < 0) MakeUseless();
        }

        private void Renderer_DrawRequestsFetching(FetchingDrawRequestsEventArgs args)
        {
            var drawArgs = args.DefaultDrawArgs;

            drawArgs.Apply(Entity.Transform);
            drawArgs.Apply(_surikenTex);
            drawArgs.Scale = new Vector2(1f / _surikenTex.Width, 1f / _surikenTex.Height);

            if (!args.IsDrawRequestOutOfScreen(drawArgs))
            {
                args.DrawRequests.Add(drawArgs);
            }
            else
            {
                //Debugger.Break();
                MakeUseless();
            }
        }

        private void MakeUseless()
        {
            BecomeUseless?.Invoke();
        }

        public TransformEntity Entity;

        Texture2D _surikenTex;
        SpriteRendererComponent _surikenRenderer;

        float _moveSpeed = 12f;
        float _rotationSpeed = 20f;
        float _disappearTime = 5f; // seconds

        public event Action? BecomeUseless;

        public void Dispose()
        {
            _surikenRenderer.DrawRequestsFetching -= Renderer_DrawRequestsFetching;
            Entity.Dispose();
        }
    }
}
