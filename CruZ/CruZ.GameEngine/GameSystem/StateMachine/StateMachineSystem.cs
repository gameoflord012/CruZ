using CruZ.GameEngine.GameSystem.Input;

namespace CruZ.GameEngine.GameSystem.StateMachine
{
    internal class StateMachineSystem : EntitySystem
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            _inputSystem = AttachedWorld.GetSystem<InputSystem>();
        }

        protected override void OnUpdate(SystemEventArgs args)
        {
            base.OnUpdate(args);

            foreach(var machine in
                args.ActiveEntities.GetAllComponents<StateMachineComponent>())
            {
                machine.Update(new(args.GameTime, _inputSystem.InputInfo));
            }
        }

        protected override void OnDraw(SystemEventArgs args)
        {
            base.OnDraw(args);

            foreach(var machine in
                args.ActiveEntities.GetAllComponents<StateMachineComponent>())
            {
                machine.Draw(args.GameTime);
            }
        }

        private InputSystem _inputSystem;
    }
}
