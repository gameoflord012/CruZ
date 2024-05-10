using CruZ.GameEngine.Input;

using Microsoft.Xna.Framework;

namespace CruZ.GameEngine.GameSystem.Script
{
    public record ScriptUpdateArgs(GameTime GameTime, IInputInfo InputInfo);
}
