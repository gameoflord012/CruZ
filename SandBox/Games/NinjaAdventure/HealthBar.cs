using System.Collections.Generic;
using System.Numerics;

using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.Render;
using CruZ.GameEngine.GameSystem.Scene;

using Microsoft.Xna.Framework.Graphics;

using NinjaAdventure.Graphics;

namespace NinjaAdventure
{
    internal class HealthBar : ScriptingEntity
    {
        public HealthBar(GameScene gameScene) : base(gameScene)
        {
            _texture = GameApplication.Resource.Load<Texture2D>("art\\healthbar.png");

            _renderer = new ProgressSpriteRenderer();
            {
                _renderer.DrawRequestsFetching += Renderer_DrawRequestsFetching;
            }
            Entity.AddComponent(_renderer);

            Entity.Scale = new Vector2(2.5f / _texture.Bounds.Width, 3.5f / _texture.Bounds.Width);
        }

        private void Renderer_DrawRequestsFetching(List<DrawRequestBase> requests)
        {
            var drawArgs = new SpriteDrawArgs();
            drawArgs.Apply(Entity.Transform);
            drawArgs.Apply(_texture);
            requests.Add(new SpriteDrawRequest(drawArgs));
        }

        ProgressSpriteRenderer _renderer;
        Texture2D _texture;

        public override void Dispose()
        {
            base.Dispose();

            _renderer.DrawRequestsFetching -= Renderer_DrawRequestsFetching;
        }
    }
}
