using Genbox.VelcroPhysics.Dynamics;

namespace CruZ.GameEngine.GameSystem.Physic
{
    public class PhysicManager
    {
        public static World World => PhysicSystem.Instance.World;
    }
}
