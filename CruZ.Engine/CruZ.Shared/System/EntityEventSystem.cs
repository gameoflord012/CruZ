using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System.Diagnostics;

namespace CruZ.Components
{
    public class EntityEventSystem : EntitySystem, IDrawSystem, IUpdateSystem
    {
        public EntityEventSystem() : base(Aspect.All(typeof(EntityEventComponent)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _entityEventComponentMapper = mapperService.GetMapper<EntityEventComponent>();
        }

        public void Draw(GameTime gameTime)
        {
            ProcessStartEvent();

            foreach (var entityEvent in this.GetAllComponents(_entityEventComponentMapper))
            {
                entityEvent.InvokeOnDraw(gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            ProcessStartEvent();

            foreach (var entityEvent in this.GetAllComponents(_entityEventComponentMapper))
            {
                entityEvent.InvokeOnUpdate(gameTime);
            }
        }

        public void ProcessStartEvent()
        {
            if (_isStartEventCalled) return;
            _isStartEventCalled = true;

            foreach (var entityEvent in this.GetAllComponents(_entityEventComponentMapper))
            {
                entityEvent.InvokeOnStart();
            }
        }

        private ComponentMapper<EntityEventComponent> _entityEventComponentMapper;
        bool _isStartEventCalled = false;
    }
}