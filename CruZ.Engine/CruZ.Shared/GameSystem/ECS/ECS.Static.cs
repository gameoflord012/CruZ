using CruZ.ECS;
using CruZ.Exception;
using MonoGame.Extended.Entities;

namespace CruZ.GameSystem
{
    public partial class ECS
    {
        public static void CreateContext(IECSContextProvider contextProvider)
        {
            _instance = new ECS(contextProvider);
        }

        private static ECS? _instance;

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