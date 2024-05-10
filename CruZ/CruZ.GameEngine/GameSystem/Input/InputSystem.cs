using CruZ.GameEngine.Input;

namespace CruZ.GameEngine.GameSystem.Input
{
    internal class InputSystem : EntitySystem
    {
        protected override void OnUpdate(SystemEventArgs args)
        {
            base.OnUpdate(args);
            _inputInfo = GameInput.GetLastInfo();
        }

        public IInputInfo InputInfo
        { get => _inputInfo; }

        private IInputInfo _inputInfo;

    }
}
