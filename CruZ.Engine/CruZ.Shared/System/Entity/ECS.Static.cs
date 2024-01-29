using CruZ.Components;
using CruZ.Exception;
using MonoGame.Extended.Entities;

namespace CruZ.Systems
{
    public partial class ECS
    {
        public static void SetContext(IECSContextProvider contextProvider)
        {
            _instance = new ECS(contextProvider);
        }

        private static ECS? _instance;
        public static ECS Instance { get => _instance ?? throw new MissingContextException(typeof(ECS)); }

        public static World World { get => Instance._world; }

        public static TransformEntity CreateEntity()
        {
            return World.CreateTransformEntity();
        }
    }
}