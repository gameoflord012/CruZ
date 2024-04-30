using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Genbox.VelcroPhysics.Dynamics;

namespace CruZ.GameEngine.GameSystem.Physic
{
    public class PhysicManager
    {
        public static World World => PhysicSystem.Instance.World;
    }
}
