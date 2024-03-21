using CruZ.Common.ECS.Ultility;

using Microsoft.Xna.Framework;

using MonoGame.Extended.Entities;

namespace CruZ.Common.ECS
{
    interface IECSController
    {
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
        void Initialize();
    }

    public partial class ECSManager : IECSController
    {
        private ECSManager() { }

        void IECSController.Initialize()
        {
            _world = new WorldBuilder().
                //AddSystem(new EntityEventSystem()).
                AddSystem(new RenderSystem()).
                AddSystem(new AnimatedSystem()).
                //AddSystem(new PhysicSystem()).
                AddSystem(new EntityScriptSystem()).
                Build();
        }

        void IECSController.Update(GameTime gameTime)
        {
            _world.Update(gameTime);
        }

        void IECSController.Draw(GameTime gameTime)
        {
            _world.Draw(gameTime);
        }

        World _world;

        internal static IECSController CreateContext()
        {
            return _instance = new ECSManager();
        }

        private static ECSManager? _instance;

        /// <summary>
        /// Not good idea to call this without proper memory manage
        /// </summary>
        /// <returns></returns>
        internal static TransformEntity CreateEntity()
        {
            return _instance._world.CreateTransformEntity();
        }

        internal static void Destroy(Entity entity)
        {
            _instance._world.DestroyEntity(entity);
        }
    }
}