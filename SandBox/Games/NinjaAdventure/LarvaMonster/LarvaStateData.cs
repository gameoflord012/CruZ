using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.Animation;
using CruZ.GameEngine.GameSystem.Physic;
using CruZ.GameEngine.GameSystem.StateMachine;

using Genbox.VelcroPhysics.Dynamics;

namespace NinjaAdventure
{
    internal class LarvaStateData : StateData
    {
        public PhysicBodyComponent Physic;
        public HealthComponent Health;
        public AnimationComponent Animation;

        public bool IsUseless;

        public List<Body> HitBodies = [];
        public Transform? Follow;
    }
}
