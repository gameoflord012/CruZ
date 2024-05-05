using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem.StateMachine;

using Microsoft.Xna.Framework.Audio;

namespace NinjaAdventure.Ninja
{
    internal class NinjaGetHitState : BasicState
    {
        protected override void OnTransitionChecking()
        {
            base.OnTransitionChecking();

            if(Machine.CurrentState == typeof(NinjaDieState)) return;

            var monsterCount = GetData<int>("MonsterCount");
            if (monsterCount > 0 && GetData<float>("TotalGameTime") - _lastHitTime > _timeBetweenHit)
            {
                Machine.SetNextState(typeof(NinjaGetHitState));
            }
        }

        protected override void OnAdded()
        {
            base.OnAdded();
            _health = GetData<HealthComponent>("HealthComponent");
        }

        protected override string? GetStateEnterSoundResource()
        {
            return "sound\\ninja-hurt.ogg";
        }

        protected override void OnStateEnter()
        {
            base.OnStateEnter();

            _health.Current -= 5;
            _lastHitTime = GetData<float>("TotalGameTime");

            if(_health.Current > 0)
                Machine.SetNextState(typeof(NinjaMovingState));
            else
                Machine.SetNextState(typeof(NinjaDieState));
        }

        float _timeBetweenHit = 1f;
        float _lastHitTime = float.NegativeInfinity;

        HealthComponent _health;   
    }
}
