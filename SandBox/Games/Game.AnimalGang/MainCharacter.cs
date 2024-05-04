using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using CruZ.GameEngine.GameSystem.ECS;

using System;
using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.Script;
using CruZ.GameEngine.GameSystem.Animation;
using CruZ.GameEngine.Input;
using CruZ.GameEngine.Utility;

namespace AnimalGang
{


    public class MainCharacter : ScriptComponent
    {
        public float Speed { get => _speed; set => _speed = value; }

        protected override void OnAttached(TransformEntity entity)
        {
            base.OnAttached(entity);
        }

        protected override void OnComponentChanged(ComponentCollection comps)
        {
            comps.TryGetComponent(out _sprite);
            comps.TryGetComponent(out _animation);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            #region AttackLogics
            //if (InputManager.Info.Keyboard.IsKeyDown(Keys.Space))
            //{
            //    _attackTimer = 0;
            //    _animation.SelectTag("player-sword-attack").Play("attack");
            //}

            //if (_attackTimer < _attackDuration)
            //{
            //    _attackTimer += (float)gameTime.ElapsedGameTime.TotalGameTime;
            //    return;
            //} 
            #endregion

            if(_remainDis >= 0)
            {
                _remainDis -= _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                AttachedEntity.Transform.Position +=
                    _moveDir * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                SnapPosition();

                _moveDir = GetMovingInput();

                if(_moveDir.SqrMagnitude() > 0.1)
                {
                    _remainDis = 1;
                }
            }

            #region AnimationLogics
            //if (_moveDir.SqrMagnitude() > 0.1f)
            //{
            //    _animation.SelectTag("player-normal").Play("walk");

            //    if (MathF.Abs(_moveDir.X) > 0.1)
            //    {
            //        _sprite.FlipHorizontally = _moveDir.X < 0;
            //    }
            //}
            //else
            //{
            //    _animation.SelectTag("player-sword-idle").Play("idle");
            //} 
            #endregion
        }

        private void SnapPosition()
        {
            var px = AttachedEntity.Transform.Position.X;
            var py = AttachedEntity.Transform.Position.Y;

            px = MathF.Ceiling(px) - 0.5f;
            py = MathF.Ceiling(py) - 0.5f;

            AttachedEntity.Transform.Position = new(px, py);
        }

        private Vector2 GetMovingInput()
        {
            Vector2 moveDir = Vector2.Zero;

            if (InputManager.Info.Keyboard.IsKeyDown(Keys.A))
            {
                moveDir = new Vector2(-1, 0);
            }
            if (InputManager.Info.Keyboard.IsKeyDown(Keys.D))
            {
                moveDir = new Vector2(1, 0);
            }
            if (InputManager.Info.Keyboard.IsKeyDown(Keys.S))
            {
                moveDir = new Vector2(0, 1);
            }
            if (InputManager.Info.Keyboard.IsKeyDown(Keys.W))
            {
                moveDir = new Vector2(0, -1);
            }

            return moveDir;
        }

        private Vector2 GetSnapPos()
        {
            return new(
                FunMath.RoundInt(AttachedEntity.Transform.Position.X),
                FunMath.RoundInt(AttachedEntity.Transform.Position.Y));
        }

        AnimationComponent _animation;
        SpriteRendererComponent _sprite;

        float _speed = 6;
        Vector2 _moveDir;
        float _remainDis = 0;
        bool _moving = false;

        float _attackDuration = 0.2f;
        float _attackTimer = 9999999f;
    }
}