using CruZ.Framework.GameSystem.ECS;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.Common.ECS
{
    class AnimatedSystem : EntitySystem
    {
        public override void Initialize(/*IComponentMapperService mapperService*/)
        {
            //_spriteRendererMapper = mapperService.GetMapper<AnimationComponent>();
            //_spriteBatch = GameApplication.GetSpriteBatch();
        }

        public virtual void Update(GameTime gameTime)
        {
            //foreach (var spriteRenderer in this.GetAllComponents(_spriteRendererMapper))
            //{
            //    spriteRenderer.Update(gameTime);
            //}
        }

        SpriteBatch _spriteBatch;
        //ComponentMapper<AnimationComponent> _spriteRendererMapper;
    }
}