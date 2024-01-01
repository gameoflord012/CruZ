using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using System.Diagnostics;

namespace CruZ.Components
{
    public class AnimatedSpriteComponent : ISpriteBatchDrawable, IUpdateable, IComponentAddedCallback
    {
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
        public SpriteSheet SpriteSheed { get => _spriteSheet; set => _spriteSheet = value; }
        public AnimatedSprite AnimatedSprite { get => _animatedSprite; }
        private TransformEntity? _attachedEntity;
    }
}