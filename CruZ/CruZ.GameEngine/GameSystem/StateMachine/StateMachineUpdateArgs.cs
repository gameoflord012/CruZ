using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CruZ.GameEngine.Input;

using Microsoft.Xna.Framework;

namespace CruZ.GameEngine.GameSystem.StateMachine
{
    public record class StateUpdateArgs(GameTime GameTime, IInputInfo InputInfo);
}
