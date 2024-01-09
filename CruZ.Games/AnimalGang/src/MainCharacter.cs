using CruZ.Components;
using CruZ.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CruZ.Games.AnimalGang
{
    public class MainCharacter : EntityScript 
    {
        protected override void OnInit()
        {
            _sprite = AttachedEntity.GetComponent<SpriteComponent>();
            _animation = AttachedEntity.GetComponent<AnimationComponent>();
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (Input.KeyboardState.IsKeyDown(Keys.Space))
            {
                _attackTimer = 0;
                _animation.SelectPlayer("player-sword-attack").Play("attack");
            }

            if (_attackTimer < _attackDuration)
            {
                _attackTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                return;
            }

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

            if (dir.SqrMagnitude() > 0.1)
            {
                _animation.SelectPlayer("player-normal").Play("walk");
                _sprite.Flip = dir.X < 0;
            }
            else
            {
                _animation.SelectPlayer("player-sword-idle").Play("idle");
            }   

            AttachedEntity.Transform.Position += dir * _speed;
        }

        AnimationComponent _animation;
        SpriteComponent _sprite;

        float _attackDuration = 0.2f;
        float _attackTimer = 9999999f;
        float _speed = 6;
    }
}