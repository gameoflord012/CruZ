using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Audio;

namespace CruZ.GameEngine.GameSystem.StateMachine
{
    public class BasicState : StateBase
    {
        protected virtual string? GetStateEnterSoundResource()
        {
            return default;
        }

        protected override void OnAdded()
        {
            base.OnAdded();

            var resource = GetStateEnterSoundResource();
            if (!string.IsNullOrEmpty(resource))
                _stateBeginSound = GameApplication.Resource.Load<SoundEffect>(resource);
        }

        protected override void OnStateEnter()
        {
            base.OnStateEnter();
            _stateBeginSound?.Play();
        }

        SoundEffect? _stateBeginSound;
    }
}
