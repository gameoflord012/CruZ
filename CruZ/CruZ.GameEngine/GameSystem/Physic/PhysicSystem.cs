using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.MonoGame.DebugView;

using Microsoft.Xna.Framework;

namespace CruZ.GameEngine.GameSystem.Physic
{
    internal class PhysicSystem : EntitySystem
    {
        public PhysicSystem()
        {
            _physicWorld = new(Vector2.Zero);
            _debugView = new(_physicWorld);
        }

        public override void OnInitialize()
        {
            _debugView.LoadContent(
                GameApplication.GetGraphicsDevice(), 
                GameApplication.GetContentManager(),
                GameContext.InternalResource.ContentDir);
        }

        protected override void OnUpdate(EntitySystemEventArgs args)
        {
            _physicWorld.Step(args.GameTime.GetElapsedSeconds());
        }

        DebugView _debugView;
        World _physicWorld;
    }
}
