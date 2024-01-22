using CruZ.Components;
using CruZ.Systems;
using CruZ.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

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

            //_isArrvied = Arrived();

            //if (_isArrvied)
            //{
            //    _moveDir = GetMovingInput();
            //    _animation.SelectPlayer("player-sword-idle").Play("idle");
            //}
            //else
            //{
            //    _animation.SelectPlayer("player-normal").Play("walk");
            //    _sprite.Flip = _moveDir.X < 0;
            
            //    AttachedEntity.Transform.Position += _moveDir * _speed;
            //}

            _moveDir = GetMovingInput();

            if(_moveDir.SqrMagnitude() > 0.1f)
            {
                _animation.SelectPlayer("player-normal").Play("walk");

                if(MathF.Abs(_moveDir.X) > 0.1)
                {
                    _sprite.Flip = _moveDir.X < 0;
                }
            }
            else
            {
                _animation.SelectPlayer("player-sword-idle").Play("idle");
            }

            AttachedEntity.Transform.Position += 
                _moveDir * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private Vector3 GetMovingInput()
        {
            Vector3 moveDir = Vector3.Zero;

            if (Input.KeyboardState.IsKeyDown(Keys.A))
            {
                moveDir += new Vector3(-1, 0);
            }
            if (Input.KeyboardState.IsKeyDown(Keys.D))
            {
                moveDir += new Vector3(1, 0);
            }
            if (Input.KeyboardState.IsKeyDown(Keys.S))
            {
                moveDir += new Vector3(0, 1);
            }
            if (Input.KeyboardState.IsKeyDown(Keys.W))
            {
                moveDir += new Vector3(0, -1);
            }

            return moveDir;
        }

        //bool Arrived()
        //{
        //    var delt = GetSnapPos() - AttachedEntity.Transform.Position;
        //    return delt.SqrMagnitude() > 0.1;
        //}

        private Vector3 GetSnapPos()
        {
            return new(
                FunMath.RoundInt(AttachedEntity.Transform.Position.X),
                FunMath.RoundInt(AttachedEntity.Transform.Position.Z),
                FunMath.RoundInt(AttachedEntity.Transform.Position.Y));
        }

        AnimationComponent _animation;
        SpriteComponent _sprite;

        float _speed = 6;
        Vector3 _moveDir;
        bool _isArrvied = false;

        float _attackDuration = 0.2f;
        float _attackTimer = 9999999f;
    }
}