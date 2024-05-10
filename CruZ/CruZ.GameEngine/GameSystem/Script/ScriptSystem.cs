using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.Input;
using CruZ.GameEngine.Input;

using Microsoft.Xna.Framework;

namespace CruZ.GameEngine.GameSystem.Script
{
    internal class ScriptSystem : EntitySystem
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            _inputSystem = AttachedWorld.GetSystem<InputSystem>();
        }

        protected override void OnDraw(SystemEventArgs args)
        {
            foreach (var script in
                args.ActiveEntities.GetAllComponents<ScriptComponent>())
            {
                script?.InternalDraw(args.GameTime);
            }
        }

        protected override void OnUpdate(SystemEventArgs args)
        {
            foreach (var script in
                args.ActiveEntities.GetAllComponents<ScriptComponent>())
            {
                script?.InternalUpdate(new(args.GameTime, _inputSystem.InputInfo));
            }
        }

        private InputSystem _inputSystem;
    }
}
