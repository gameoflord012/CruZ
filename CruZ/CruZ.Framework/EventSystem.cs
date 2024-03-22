//using Microsoft.Xna.Framework;
//using MonoGame.Extended.Entities;
//using MonoGame.Extended.Entities.Systems;
//using System.Diagnostics;

//namespace CruZ.Components
//{
//    public class EventSystem : EntitySystem, IDrawSystem, IUpdateSystem
//    {
//        public EventSystem() : base(Aspect.All(typeof(EntityEventComponent)))
//        {
//        }

//        public override void CreateContext(IComponentMapperService mapperService)
//        {
//            _entityEventComponentMapper = mapperService.GetMapper<EntityEventComponent>();
//        }

//        public void OnDraw(GameTime gameTime)
//        {
//            foreach (var entityEvent in this.GetAllComponents(_entityEventComponentMapper))
//            {
//                entityEvent.InvokeOnDraw(gameTime);
//            }
//        }

//        public void OnUpdate(GameTime gameTime)
//        {
//            foreach (var entityEvent in this.GetAllComponents(_entityEventComponentMapper))
//            {
//                entityEvent.InvokeOnUpdate(gameTime);
//            }
//        }

//        private ComponentMapper<EntityEventComponent> _entityEventComponentMapper;
//    }
//}