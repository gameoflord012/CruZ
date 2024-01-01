using CruZ.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace CruZ.Game
{
    public class MainCharacter : EntityTemplate
    {
        public override void GetInstruction(IBuildInstruction instruction)
        {
            instruction.RequireComponent(typeof(SpriteComponent));
        }

        public override void Initialize(TransformEntity relativeRoot)
        {
            base.Initialize(relativeRoot);

            Entity.IsActive = true;

            _sprite = Entity.GetComponent<SpriteComponent>();
            _sprite.LoadTexture("image");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Entity.Transform.Position += Microsoft.Xna.Framework.Vector3.Up * 6;
        }

        SpriteComponent _sprite;
    }
}