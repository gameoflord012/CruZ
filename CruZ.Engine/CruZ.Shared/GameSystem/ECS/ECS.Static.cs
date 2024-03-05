using CruZ.Common.ECS;

using MonoGame.Extended.Entities;

namespace CruZ.Common.ECS
{
    public partial class ECSManager
    {
        public static void CreateContext(IECSContextProvider contextProvider)
        {
            _instance = new ECSManager(contextProvider);
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