using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;

namespace CruZ.Components
{
    public class AnimatedSpriteComponent : ISpriteBatchDrawable, IUpdateable, IComponentAddedCallback
    {
        public AnimatedSpriteComponent(SpriteSheet spriteShit)
        {
            _spriteSheet = spriteShit;
            _animatedSprite = new AnimatedSprite(_spriteSheet);
        }

        private Microsoft.Xna.Framework.Vector3 GetRenderPosition()
        {
            var e = TransformEntity.GetEntity(this);
            return e.Transform.Position;
        }

        public void Draw(SpriteBatch spriteBatch, Matrix transformMatrix)
        {
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

        MonoGame.Extended.Sprites.AnimatedSprite _animatedSprite;
        SpriteSheet _spriteSheet;
        public SpriteSheet SpriteSheed { get => _spriteSheet; set => _spriteSheet = value; }
        public AnimatedSprite AnimatedSprite { get => _animatedSprite; }

        private TransformEntity _attachedEntity;
    }
}