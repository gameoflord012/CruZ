using CruZ.Components;
using CruZ.Utility;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace CruZ.Systems
{
    public class EntityScriptSystem : EntitySystem, IUpdateSystem, IDrawSystem
    {
        public EntityScriptSystem() : base(Aspect.All(typeof(EntityScript)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _scriptMapper = mapperService.GetMapper<EntityScript>();
        }

        public void Draw(GameTime gameTime)
        {
            foreach(var script in this.GetAllComponents(_scriptMapper))
            {
                script.InternalDraw(gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var script in this.GetAllComponents(_scriptMapper))
            {
                script.InternalUpdate(gameTime);
            }
        }

        ComponentMapper<EntityScript> _scriptMapper;
    }
}