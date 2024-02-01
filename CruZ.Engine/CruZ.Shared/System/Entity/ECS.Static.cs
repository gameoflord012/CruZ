using CruZ.Components;
using CruZ.Exception;
using MonoGame.Extended.Entities;

namespace CruZ.Systems
{
    public partial class ECS
    {
        public static void CreateContext(IECSContextProvider contextProvider)
        {
            _Instance = new ECS(contextProvider);
        }

        private static ECS? _Instance;

        public static World World { get => _Instance._world; }

        public static TransformEntity CreateEntity()
        {
            return World.CreateTransformEntity();
        }
    }
}