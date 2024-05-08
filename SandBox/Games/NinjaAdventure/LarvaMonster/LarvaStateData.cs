using System.Collections.Generic;

using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.Animation;
using CruZ.GameEngine.GameSystem.Physic;
using CruZ.GameEngine.GameSystem.StateMachine;

using Genbox.VelcroPhysics.Dynamics;

namespace NinjaAdventure
{
    internal class LarvaStateData : StateData
    {
        public LarvaStateData(
            PhysicBodyComponent physic,
            HealthComponent health,
            AnimationComponent animation,
            LarvaMonster larva)
        {
            (Physic, Health, Animation, Larva) = (physic, health, animation, larva);
            HitBodies = [];
        }

        public void Reset(Transform? follow)
        {
            Health.Current = 30;
            HitBodies.Clear();
            Follow = follow;
        }

        public PhysicBodyComponent Physic
        {
            get;
            private set;
        }
        public HealthComponent Health
        {
            get;
            private set;
        }
        public AnimationComponent Animation
        {
            get;
            private set;
        }
        public LarvaMonster Larva
        {
            get;
            private set;
        }

        public List<Body> HitBodies;

        public Transform? Follow;
    }
}
