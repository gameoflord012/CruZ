using CruZ.Framework.GameSystem.ECS;

using Microsoft.Xna.Framework;

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
            _world = new World();
            _world.
                AddSystem(new RenderSystem()).
                AddSystem(new AnimatedSystem()).
                AddSystem(new EntityScriptSystem());
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
            return new TransformEntity(_instance._world);
        }
    }
}