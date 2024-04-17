using Microsoft.Xna.Framework;

namespace CruZ.Framework.GameSystem.Script
{
    internal class ScriptSystem : EntitySystem
    {
        protected override void OnDraw(EntitySystemEventArgs args)
        {
            foreach (var script in
                args.ActiveEntities.GetAllComponents<ScriptComponent>())
            {
                script?.InternalDraw(args.GameTime);
            }
        }

        protected override void OnUpdate(EntitySystemEventArgs args)
        {
            foreach (var script in
                args.ActiveEntities.GetAllComponents<ScriptComponent>())
            {
                script?.InternalUpdate(args.GameTime);
            }
        }
    }
}