using CruZ.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System;

namespace CruZ.Systems
{
    class AnimatedSystem : EntitySystem, IDrawSystem, IUpdateSystem
    {
#pragma warning disable CS8618
        public AnimatedSystem() : base(Aspect.All(typeof(AnimatedSpriteComponent)))
        {
        }
#pragma warning restore CS8618

        public override void Initialize(IComponentMapperService mapperService)
        {
            _spriteRendererMapper = mapperService.GetMapper<AnimatedSpriteComponent>();
            _spriteBatch = new SpriteBatch(Core.Instance.GraphicsDevice);
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var animatedSprite in this.GetAllComponents(_spriteRendererMapper))
            {
                animatedSprite.Draw(_spriteBatch, Camera.Main.ViewMatrix());
            }
        }

        public virtual void Update(GameTime gameTime) 
        {
            foreach (var spriteRenderer in this.GetAllComponents(_spriteRendererMapper))
            {
                spriteRenderer.Update(gameTime);
            }
        }

        SpriteBatch _spriteBatch;
        ComponentMapper<AnimatedSpriteComponent> _spriteRendererMapper;
    }
}