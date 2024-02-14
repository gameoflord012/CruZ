using CruZ.Components;
using CruZ.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System;

namespace CruZ.Systems
{
    class AnimatedSystem : EntitySystem, IUpdateSystem
    {
#pragma warning disable CS8618
        public AnimatedSystem() : base(Aspect.All(typeof(AnimationComponent)))
        {
        }
#pragma warning restore CS8618

        public override void Initialize(IComponentMapperService mapperService)
        {
            _spriteRendererMapper = mapperService.GetMapper<AnimationComponent>();
            _spriteBatch = GameApplication.GetSpriteBatch();
        }

        //public void OnDraw(GameTime gameTime)
        //{
        //    foreach (var animatedSprite in this.GetAllComponents(_spriteRendererMapper))
        //    {
        //        animatedSprite.OnDraw(_spriteBatch, Camera.Main.ViewMatrix());
        //    }
        //}

        public virtual void Update(GameTime gameTime) 
        {
            foreach (var spriteRenderer in this.GetAllComponents(_spriteRendererMapper))
            {
                spriteRenderer.Update(gameTime);
            }
        }

        SpriteBatch _spriteBatch;
        ComponentMapper<AnimationComponent> _spriteRendererMapper;
    }
}