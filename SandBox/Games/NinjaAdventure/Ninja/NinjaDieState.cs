using CruZ.GameEngine.GameSystem.StateMachine;

namespace NinjaAdventure.Ninja
{
    internal class NinjaDieState : BasicState<NinjaStateData>
    {
        protected override string? GetStateEnterSoundResource()
        {
            return "sound\\ninja-die.ogg";
        }
    }
}
