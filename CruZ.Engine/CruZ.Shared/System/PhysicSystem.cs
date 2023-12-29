using Box2D.NetStandard.Dynamics.World;
using CruZ_Engine.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace CruZ_Engine.Systems
{
    class PhysicSystem : EntityUpdateSystem
    {
        public PhysicSystem() : base(Aspect.All(typeof(PhysicBody)))
        {
            _physicWorld = new();
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _physicBodyMapper = mapperService.GetMapper<PhysicBody>();

            foreach(var entityId in ActiveEntities)
            {
                PhysicBody body = _physicBodyMapper.Get(entityId);
                if(!body.IsInitialize)
                {
                    body.Initialize(_physicWorld);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            _physicWorld.Step((float)gameTime.ElapsedGameTime.TotalSeconds, 6, 2);
        }

        Box2D.NetStandard.Dynamics.World.World _physicWorld;
        ComponentMapper<PhysicBody> _physicBodyMapper;
    }
}