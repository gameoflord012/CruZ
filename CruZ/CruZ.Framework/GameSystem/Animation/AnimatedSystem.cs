using System.Linq;

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
            foreach (var animation in
                args.ActiveEntities.GetAllComponents<AnimationComponent>())
            {
                animation?.Update(args.GameTime);
            }
        }
    }
}