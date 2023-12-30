
using Assimp;
using CruZ.Components;
using Microsoft.Xna.Framework;

namespace CruZ.Game
{
    public class MainCharacter : EntityTemplate
    {
        public override void Initialize(TransformEntity e)
        {
            e.AddComponent(new SpriteComponent("image"));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            AppliedEntity.Transform.Position += Microsoft.Xna.Framework.Vector3.Left * 6;
        }
    }
}