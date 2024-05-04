using CruZ.GameEngine.Utility;

using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.MonoGame.DebugView;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.GameEngine.GameSystem.Physic
{
    internal class PhysicSystem : EntitySystem
    {
        private PhysicSystem()
        {
            _physicWorld = new(Vector2.Zero);
            _debugView = new(_physicWorld);
        }

        public override void OnInitialize()
        {
            _gd = GameApplication.GetGraphicsDevice();

            _debugView.LoadContent(
                _gd,
                GameApplication.GetContentManager(),
                GameApplication.InternalResource.ContentDir);
        }

        protected override void OnUpdate(EntitySystemEventArgs args)
        {
            _physicWorld.Step(args.GameTime.DeltaTime());

            foreach (var physic in args.ActiveEntities.GetAllComponents<PhysicBodyComponent>())
            {
                physic.Update(args.GameTime);
            }
        }

        protected override void OnDraw(EntitySystemEventArgs args)
        {
            _gd.SetRenderTarget(RenderTargetSystem.PhysicRT);
            _gd.Clear(Color.Transparent);

            _debugView.RenderDebugData(
                Camera.Main.ProjectionMatrix(),
                Camera.Main.ViewMatrix());

            _gd.SetRenderTarget(null);
        }

        public override void Dispose()
        {
            base.Dispose();
            IsDisposed = true;
        }

        GraphicsDevice _gd;

        internal bool IsDisposed { get; private set; }

        DebugView _debugView;
        public World World { get => _physicWorld; }
        
        World _physicWorld;

        private static PhysicSystem? s_instance;

        internal static PhysicSystem Instance
        {
            get => s_instance ?? throw new System.InvalidOperationException();
        }

        internal static PhysicSystem CreateContext()
        {
            if (s_instance != null && !s_instance.IsDisposed) throw new System.InvalidOperationException();
            return s_instance = new PhysicSystem();
        }
    }
}
