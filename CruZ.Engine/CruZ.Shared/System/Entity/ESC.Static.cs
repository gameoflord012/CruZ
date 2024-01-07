using CruZ.Components;
using MonoGame.Extended.Entities;

namespace CruZ.Systems
{
    public partial class ECS
    {
        private static ECS? _instance;
        public static ECS Instance { get => _instance ??= new ECS(); }

        public static World World { get => Instance._world; }

        public static TransformEntity CreateEntity()
        {
            return World.CreateTransformEntity();
        }
    }
}