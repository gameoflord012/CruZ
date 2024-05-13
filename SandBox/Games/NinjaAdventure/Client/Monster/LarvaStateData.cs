using System.Collections.Generic;

using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.Animation;
using CruZ.GameEngine.GameSystem.Physic;
using CruZ.GameEngine.GameSystem.StateMachine;

using Genbox.VelcroPhysics.Dynamics;

using Microsoft.Xna.Framework;

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
            HitOrigins = [];
        }

        public void Reset(Transform? follow)
        {
            Health.Current = 30;
            HitOrigins.Clear();
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

        public Stack<Vector2> HitOrigins;

        public Transform? Follow;
    }
}
