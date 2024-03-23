using CruZ.Common;
using CruZ.Framework.GameSystem.ECS;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.Framework.GameSystem.Animation
{
    class AnimationSystem : EntitySystem
    {
        protected override void OnUpdate(EntitySystemEventArgs args)
        {
            args.Entity.TryGetComponent(out AnimationComponent? animation);
            animation?.Update(args.GameTime);
        }
    }
}