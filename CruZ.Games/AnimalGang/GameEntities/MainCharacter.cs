using CruZ.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

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
            _e.Transform.Position += Microsoft.Xna.Framework.Vector3.Up * 6;

        }

        SpriteComponent _sprite;
        TransformEntity _e;
    }
}