using CruZ.GameEngine.GameSystem.Animation;
using CruZ.GameEngine.GameSystem.StateMachine;

using Microsoft.Xna.Framework;

using MonoGame.Aseprite;

namespace NinjaAdventure.Ninja
{
    internal class NinjaDieState : BasicState<NinjaStateData>
    {
        protected override string? StateEnterSoundResource => "sound\\ninja-die.ogg";

        protected override void OnAdded()
        {
            base.OnAdded();
            _animation = StateData.Animation;
            _animation.LoadAnimationFile("anim\\Ninja\\smoke-effect.aseprite", "smoke");
        }

        protected override void OnStateEnter()
        {
            base.OnStateEnter();
            
            StateData.Health.ShouldDisplay = false;
            _animation.Scale = Vector2.One * 3;
            _animation.Offset = new Vector2(-0.3f, 0.4f);

            _animation.Play("smoke-sweep", 1);
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
            _animation.Scale = Vector2.One;
            _animation.Offset = Vector2.Zero;
        }

        AnimationComponent _animation;
    }
}
