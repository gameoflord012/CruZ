﻿using CruZ.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System;

namespace CruZ.Systems
{
    class AnimatedSystem : EntitySystem, IDrawSystem, IUpdateSystem
    {
        public AnimatedSystem() : base(Aspect.All(typeof(AnimatedSpriteComponent)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _spriteRendererMapper = mapperService.GetMapper<AnimatedSpriteComponent>();
            _spriteBatch = new SpriteBatch(MGWrapper.Instance().GraphicsDevice);
            _core = MGWrapper.Instance();
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
        MGWrapper _core;
    }
}