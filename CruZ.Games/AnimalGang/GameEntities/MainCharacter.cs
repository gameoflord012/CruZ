using CruZ.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace CruZ.Game
{
    public class MainCharacter : EntityTemplate
    {
        public override object[] InitialComponents()
        {
            return
                [
                    new SpriteComponent(),
                ];
        }

        public override void Initialize(TransformEntity e)
        {
            _sprite = AppliedEntity.GetComponent<SpriteComponent>();
            _sprite.LoadTexture("image");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            AppliedEntity.Transform.Position += Microsoft.Xna.Framework.Vector3.Up * 6;
        }

        SpriteComponent _sprite;
    }
}