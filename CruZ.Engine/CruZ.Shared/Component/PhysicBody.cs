using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.Fixtures;
using Box2D.NetStandard.Dynamics.World;
using CruZ.Systems;

namespace CruZ.Components
{
    class PhysicBody
    {
        public void Initialize(World world)
        {
            _body = world.CreateBody(new BodyDef());
            _fixture = _body.CreateFixture(new FixtureDef());
        }

        Fixture _fixture;
        Body _body;
        public bool IsInitialize { get => _body != null; }
    }
}
