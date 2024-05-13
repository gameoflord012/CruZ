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
            ShowDebug = true;
        }

        public override void OnInitialize()
        {
            _gd = GameApplication.GetGraphicsDevice();

            _debugView.LoadContent(
                _gd,
                GameApplication.GetContentManager(),
                GameApplication.InternalResource.ContentDir);
        }

        protected override void OnUpdate(SystemEventArgs args)
        {
            _physicWorld.Step(args.GameTime.DeltaTime());

            foreach(var physic in args.ActiveEntities.GetAllComponents<PhysicBodyComponent>())
            {
                physic.Update(args.GameTime);
            }
        }

        protected override void OnDraw(SystemEventArgs args)
        {

            _gd.SetRenderTarget(RenderTargetSystem.PhysicRT);
            _gd.Clear(Color.Transparent);

            if(ShowDebug)
            {
#if CRUZ_EDITOR
                _debugView.RenderDebugData(
                Camera.Current.ProjectionMatrix(),
                Camera.Current.ViewMatrix());
#endif
            }

            _gd.SetRenderTarget(null);
        }

        public bool ShowDebug
        {
            get;
            set;
        }

        public World World
        {
            get => _physicWorld;
        }

        private GraphicsDevice _gd;
        private DebugView _debugView;
        private World _physicWorld;
        private bool _isDisposed;

        public override void Dispose()
        {
            base.Dispose();
            _isDisposed = true;
        }

        private static PhysicSystem? s_instance;

        internal static PhysicSystem Instance
        {
            get => s_instance ?? throw new System.InvalidOperationException();
        }

        internal static PhysicSystem CreateContext()
        {
            if(s_instance != null && !s_instance._isDisposed) throw new System.InvalidOperationException();
            return s_instance = new PhysicSystem();
        }
    }
}
