using CruZ.Framework.GameSystem.ECS;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.Common.ECS
{
    internal class RenderSystem : EntitySystem
    {
        public override void Initialize(/*IComponentMapperService mapperService*/)
        {
            //_rendererMapper = mapperService.GetMapper<RendererComponent>();
            _spriteBatch = GameApplication.GetSpriteBatch();
        }

        protected override void Draw(GameTime gameTime)
        {
            //var renderers = this.GetAllComponents(_rendererMapper);
            //renderers.Sort();

            //foreach (var renderer in renderers)
            //{
            //    renderer.Render(gameTime, _spriteBatch, Camera.Main.ViewProjectionMatrix());
            //}
        }

        protected override void Update(GameTime gameTime) { }

        SpriteBatch _spriteBatch;
    }
}
