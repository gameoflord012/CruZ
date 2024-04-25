using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using CruZ.GameEngine.GameSystem.Animation;
using CruZ.GameEngine.GameSystem.Render;
using CruZ.GameEngine.GameSystem.Script;
using CruZ.GameEngine.GameSystem.UI;

using Microsoft.Xna.Framework;

namespace CruZ.GameEngine.GameSystem
{
    internal class ECSManager : IDisposable
    {
        internal static event Action<ECSManager>? InstanceChanged;

        private ECSManager()
        {
            _world = new World();
            _world.
                AddSystem(new RenderSystem()).
                AddSystem(new AnimationSystem()).
                AddSystem(new ScriptSystem()).
                AddSystem(UISystem.CreateContext());
        }

        internal World World { get => _world; set => _world = value; }

        internal void Initialize()
        {
            _world.Initialize();
        }

        internal void Update(GameTime gameTime)
        {
            _world.SystemsUpdate(gameTime);
        }

        internal void Draw(GameTime gameTime)
        {
            _world.SystemsDraw(gameTime);
        }

        World _world;

        internal static ECSManager CreateContext()
        {
            if (_instance != null && !_instance._isDisposed)
                throw new InvalidOperationException("Require dispose");

            _instance = new ECSManager();
            InstanceChanged?.Invoke(_instance);
            return _instance;
        }

        private static ECSManager? _instance;

        /// <summary>
        /// Not good idea to call this without proper memory manage
        /// </summary>
        /// <returns></returns>
        internal static TransformEntity CreateTransformEntity()
        {
            return new TransformEntity(_instance._world);
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