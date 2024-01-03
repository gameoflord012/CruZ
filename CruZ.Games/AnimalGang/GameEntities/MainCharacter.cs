using CruZ.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CruZ.Game
{
    public class MainCharacter : EntityScript, IComponentReceivedCallback
    {
        public void OnComponentAdded(TransformEntity entity)
        {
            _e = entity;
            _e.RequireComponent(typeof(SpriteComponent));
            _sprite = _e.GetComponent<SpriteComponent>();

            _sprite.LoadTexture("image");
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

            _e.Transform.Position += dir * speed;
        }

        SpriteComponent _sprite;
        TransformEntity _e;
        float speed = 6;
    }
}