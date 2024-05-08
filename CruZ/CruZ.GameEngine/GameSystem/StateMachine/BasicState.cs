using Microsoft.Xna.Framework.Audio;

namespace CruZ.GameEngine.GameSystem.StateMachine
{
    public class BasicState<T> : StateBase<T> where T : StateData
    {
        protected virtual string StateEnterSoundResource { get; } = default;

        protected override void OnStateMachineAttached()
        {
            base.OnStateMachineAttached();

            var resource = StateEnterSoundResource;

            if (!string.IsNullOrEmpty(resource))
                _stateBeginSound = GameApplication.Resource.Load<SoundEffect>(resource, true);
        }

        protected override void OnStateEnter()
        {
            base.OnStateEnter();
            _stateBeginSound?.Play();
        }

        SoundEffect? _stateBeginSound;
    }
}
