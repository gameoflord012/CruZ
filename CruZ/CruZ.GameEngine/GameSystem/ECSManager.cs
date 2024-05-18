using System;
using System.Diagnostics;

using CruZ.GameEngine.GameSystem.Animation;
using CruZ.GameEngine.GameSystem.Input;
using CruZ.GameEngine.GameSystem.Physic;
using CruZ.GameEngine.GameSystem.Render;
using CruZ.GameEngine.GameSystem.Script;
using CruZ.GameEngine.GameSystem.StateMachine;
using CruZ.GameEngine.GameSystem.UI;

using Microsoft.Xna.Framework;

namespace CruZ.GameEngine.GameSystem
{
    internal partial class ECSManager : IDisposable
    {
        private ECSManager()
        {
            _world = new ECSWorld();
            _world.
                AddSystem(new InputSystem()).
                AddSystem(new ScriptSystem()).
                AddSystem(new StateMachineSystem()).
                AddSystem(PhysicSystem.CreateContext()).
                AddSystem(new AnimationSystem()).
                AddSystem(new RenderSystem()).
                AddSystem(UISystem.CreateContext()).
                AddSystem(RenderTargetSystem.CreateContext());
        }

        internal void Initialize()
        {
            _world.Initialize();
        }

        internal void Update(GameTime gameTime)
        {
            _world.UpdateSystems(gameTime);
        }

        internal void Draw(GameTime gameTime)
        {
            _world.DrawSystems(gameTime);
        }

        public void Dispose()
        {
            if(!_isDisposed)
            {
                _isDisposed = true;
                _world.Dispose();
                SetInstance(null);
            }
        }

        internal ECSWorld World
        {
            get => _world;
            set => _world = value;
        }

        private bool _isDisposed;
        private ECSWorld _world;
    }

    internal partial class ECSManager
    {
        internal static event Action<ECSManager?, ECSManager?>? InstanceChanged;

        internal static ECSManager CreateContext()
        {
            Trace.Assert(_instance == null || _instance._isDisposed);

            var instance = new ECSManager();
            SetInstance(instance);
            return instance;
        }

        private static void SetInstance(ECSManager? instance)
        {
            if(instance == _instance)
            {
                return;
            }

            var oldValue = _instance;
            _instance = instance;

            InstanceChanged?.Invoke(oldValue, _instance);
        }

        /// <summary>
        /// Not good idea to call this without proper memory manage
        /// </summary>
        /// <returns></returns>
        internal static TransformEntity CreateEntity()
        {
            var entity = new TransformEntity();
            Instance!.World.AddEntity(entity);
            return entity;
        }

        internal static ECSManager Instance
        {
            get => _instance ?? throw new NullReferenceException();
        }

        private static ECSManager? _instance;
    }
}
