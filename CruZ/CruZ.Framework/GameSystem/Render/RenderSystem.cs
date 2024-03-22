using CruZ.Common;
using CruZ.Framework.GameSystem.ECS;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.Framework.GameSystem.Render
{
    internal class RenderSystem : EntitySystem
    {
        public override void Initialize(/*IComponentMapperService mapperService*/)
        {
            //_rendererMapper = mapperService.GetMapper<RendererComponent>();
            _spriteBatch = GameApplication.GetSpriteBatch();
        }

        protected override void OnDraw(GameTime gameTime)
        {
            //var renderers = this.GetAllComponents(_rendererMapper);
            //renderers.Sort();

            //foreach (var renderer in renderers)
            //{
            //    renderer.Render(gameTime, _spriteBatch, Camera.Main.ViewProjectionMatrix());
            //}
        }

        protected override void OnUpdate(GameTime gameTime) { }

        SpriteBatch _spriteBatch;
    }
}
