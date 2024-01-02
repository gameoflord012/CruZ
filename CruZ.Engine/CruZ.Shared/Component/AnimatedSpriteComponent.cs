using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using System;
using System.Diagnostics;

namespace CruZ.Components
{
    public class AnimatedSpriteComponent : IComponent, ISpriteBatchDrawable, IComponentReceivedCallback
    {
        public SpriteSheet      SpriteSheed     { get => _spriteSheet; set => _spriteSheet = value; }
        public AnimatedSprite   AnimatedSprite  { get => _animatedSprite; }
        public Type             ComponentType   => throw new NotImplementedException();

        public AnimatedSpriteComponent() { }

        public AnimatedSpriteComponent(SpriteSheet spriteShit)
        {
            _spriteSheet = spriteShit;
            _animatedSprite = new AnimatedSprite(_spriteSheet);
        }

        public void Draw(SpriteBatch spriteBatch, Matrix transformMatrix)
        {
            Trace.Assert(_attachedEntity != null);

            spriteBatch.Begin(transformMatrix: _attachedEntity.Transform.TotalMatrix * transformMatrix);
            spriteBatch.Draw(_animatedSprite, Vector2.Zero);
            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            _animatedSprite.Update(gameTime);
        }

        public void OnComponentAdded(TransformEntity entity)
        {
            _attachedEntity = entity;
        }

        AnimatedSprite _animatedSprite;
        SpriteSheet _spriteSheet;

        private TransformEntity? _attachedEntity;
    }
}