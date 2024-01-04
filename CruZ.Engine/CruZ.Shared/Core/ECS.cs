using CruZ.Components;
using CruZ.Systems;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using System.Collections.Generic;

namespace CruZ
{
    public class ECS
    {
        private static ECS? _instance;
        public static ECS Instance { get => _instance ??= new ECS(); }

        private ECS()
        {
            Core.OnUpdate += Update;
            Core.OnDraw += Draw;

            _world = new WorldBuilder().
                AddSystem(new EntityEventSystem()).
                AddSystem(new SpriteSystem()).
                AddSystem(new AnimatedSystem()).
                AddSystem(new PhysicSystem()).
                AddSystem(new EntityScriptSystem()).
                Build();

            //_entityBuilder = new(_world);
        }

        private void Update(GameTime gameTime)
        {
            _world.Update(gameTime);
        }

        private void Draw(GameTime gameTime)
        {
            _world.Draw(gameTime);
        }

        World _world;
        //EntityBuilder _entityBuilder;

        public static World World { get => Instance._world; }
        
        public static TransformEntity CreateEntity()
        {
            return World.CreateTransformEntity();
        }

        //public static Dictionary<EntityTemplate, TransformEntity> BuildTemplate(EntityTemplate template)
        //{
        //    return Instance._entityBuilder.BuildFrom(template);
        //}
    }
}