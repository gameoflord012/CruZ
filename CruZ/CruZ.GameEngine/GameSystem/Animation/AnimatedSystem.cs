using CruZ.GameEngine.GameSystem;

namespace CruZ.GameEngine.GameSystem.Animation
{
    class AnimationSystem : EntitySystem
    {
        protected override void OnUpdate(EntitySystemEventArgs args)
        {
            foreach (var animation in
                args.ActiveEntities.GetAllComponents<AnimationComponent>())
            {
                animation?.Update(args.GameTime);
            }
        }
    }
}