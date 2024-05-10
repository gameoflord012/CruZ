using System;

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
    internal class ECSManager : IDisposable
    {
        internal static event Action<ECSManager?, ECSManager>? InstanceChanged;

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

        internal ECSWorld World { get => _world; set => _world = value; }

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

        ECSWorld _world;

        internal static ECSManager CreateContext()
        {
            if (_instance != null && !_instance._isDisposed)
                throw new InvalidOperationException("Require dispose");

            var newInstance = new ECSManager();

            InstanceChanged?.Invoke(_instance, newInstance);
            return _instance = newInstance;
        }

        private static ECSManager? _instance;

        /// <summary>
        /// Not good idea to call this without proper memory manage
        /// </summary>
        /// <returns></returns>
        internal static TransformEntity CreateEntity()
        {
            var entity = new TransformEntity();
            _instance!.World.AddEntity(entity);
            return entity;
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                _world.Dispose();
            }

        }

        bool _isDisposed = false;
    }
}
