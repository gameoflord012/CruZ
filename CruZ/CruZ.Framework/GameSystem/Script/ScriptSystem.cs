using CruZ.Framework.GameSystem.ECS;

using Microsoft.Xna.Framework;

namespace CruZ.Framework.GameSystem.Script
{
    internal class ScriptSystem : EntitySystem
    {
        protected override void OnDraw(EntitySystemEventArgs args)
        {
            args.Entity.TryGetComponent(out ScriptComponent? script);
            script?.InternalDraw(args.GameTime);
        }

        protected override void OnUpdate(EntitySystemEventArgs args)
        {
            args.Entity.TryGetComponent(out ScriptComponent? script);
            script?.InternalUpdate(args.GameTime);
        }
    }
}