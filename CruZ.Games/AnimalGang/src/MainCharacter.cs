using CruZ.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CruZ.Games.AnimalGang
{
    public class MainCharacter : EntityScript, IComponentCallback
    {
        public void OnEntityChanged(TransformEntity entity)
        {
            _e = entity;
            _e.OnDeserializationCompleted += Initialize;
        }

        private void Initialize()
        {
            _sprite = _e.GetComponent<SpriteComponent>();
            _animatedSprite = _e.GetComponent<AnimationComponent>();
            _animatedSprite.Play("walk");
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            Vector3 dir = Vector3.Zero;

            if(Input.KeyboardState.IsKeyDown(Keys.A))
            {
                dir += new Vector3(-1, 0);
            }
            if (Input.KeyboardState.IsKeyDown(Keys.D))
            {
                dir += new Vector3(1, 0);
            }
            if (Input.KeyboardState.IsKeyDown(Keys.S))
            {
                dir += new Vector3(0, 1);
            }
            if (Input.KeyboardState.IsKeyDown(Keys.W))
            {
                dir += new Vector3(0, -1);
            }

            _sprite.Flip = dir.X < 0;

            _e.Transform.Position += dir * speed;
        }

        AnimationComponent _animatedSprite;
        SpriteComponent _sprite;
        TransformEntity _e;
        float speed = 6;
    }
}