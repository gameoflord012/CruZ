using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Audio;

namespace CruZ.GameEngine.GameSystem.StateMachine
{
    internal class NinjaDieState : BasicState
    {
        protected override string? GetStateEnterSoundResource()
        {
            return "sound\\ninja-die.ogg";
        }
    }
}
