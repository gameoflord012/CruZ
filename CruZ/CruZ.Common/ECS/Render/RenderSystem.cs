using CruZ.Common.ECS.Ultility;
using CruZ.Common.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Framework.Content.Pipeline.Builder;

using System.Collections.Generic;
using System.Linq;

namespace CruZ.Common.ECS
{
    internal class RenderSystem : EntitySystem, IUpdateSystem, IDrawSystem
    {
        public RenderSystem() : base(Aspect.All(typeof(RendererComponent)))
        {
            
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _rendererMapper = mapperService.GetMapper<RendererComponent>();
            _spriteBatch = GameApplication.GetSpriteBatch();
        }

        public void Draw(GameTime gameTime)
        {
            var renderers = this.GetAllComponents(_rendererMapper);
            renderers.Sort();

            foreach (var renderer in renderers)
            {
                renderer.Render(gameTime, _spriteBatch, Camera.Main.ViewProjectionMatrix());
            }
        }

        public virtual void Update(GameTime gameTime) { }

        SpriteBatch _spriteBatch;
        ComponentMapper<RendererComponent> _rendererMapper;
    }
}
