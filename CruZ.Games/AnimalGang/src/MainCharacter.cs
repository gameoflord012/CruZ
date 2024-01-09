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
                _animation.SelectPlayer("normal-player").Play("walk");
                _sprite.Flip = dir.X < 0;
            }
            else
            {
                _animation.SelectPlayer("sword-player").Play("idle");
            }   

            AttachedEntity.Transform.Position += dir * speed;
        }

        AnimationComponent _animation;
        SpriteComponent _sprite;
        float speed = 6;
    }
}