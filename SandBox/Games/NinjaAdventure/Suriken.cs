using System.Diagnostics;

using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.ECS;
using CruZ.GameEngine.GameSystem.Render;
using CruZ.GameEngine.GameSystem.Scene;
using CruZ.GameEngine.GameSystem.Script;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NinjaAdventure
{
    internal class Suriken : IDisposable
    {
        public Suriken(GameScene gameScene, SpriteRendererComponent surikenRenderer, Vector2 origin, Vector2 directions)
        {
            Entity = gameScene.CreateEntity();
            Entity.Transform.Position = origin;
            Entity.Transform.Scale = new Vector2(0.3f, 0.3f);
            this._surikenRenderer = surikenRenderer;
            _direction = directions;

            var script = new ScriptComponent();
            {
                script.Updating += Script_Updating;
            }
            Entity.AddComponent(script);

            surikenRenderer.DrawRequestsFetching += Renderer_DrawRequestsFetching;

            _surikenTex = GameContext.GameResource.Load<Texture2D>("art\\suriken\\01.png");
        }

        private void Script_Updating(GameTime gameTime)
        {
            Entity.Transform.Rotation += _rotationSpeed * gameTime.GetElapsedSeconds();
            Entity.Transform.Position += _direction * gameTime.GetElapsedSeconds() * _moveSpeed;
        }

        private void Renderer_DrawRequestsFetching(FetchingDrawRequestsEventArgs args)
        {
            var drawArgs = args.DefaultDrawArgs;
            drawArgs.Apply(Entity);
            drawArgs.Apply(_surikenTex);

            if (!args.IsDrawRequestOutOfScreen(drawArgs))
            {
                args.DrawRequests.Add(drawArgs);
            }
            else
            {
                //Debugger.Break();
                BecomeUseless?.Invoke();
            }
        }

        public TransformEntity Entity;

        Texture2D _surikenTex;
        SpriteRendererComponent _surikenRenderer;
        Vector2 _direction;
        float _moveSpeed = 200;
        float _rotationSpeed = 20f;

        public event Action? BecomeUseless;

        public void Dispose()
        {
            _surikenRenderer.DrawRequestsFetching -= Renderer_DrawRequestsFetching;
            Entity.Dispose();
        }
    }
}
