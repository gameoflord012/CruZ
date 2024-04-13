using System;
using System.Diagnostics;

using CruZ.Framework.GameSystem.Animation;
using CruZ.Framework.GameSystem.Render;
using CruZ.Framework.GameSystem.Script;
using CruZ.Framework.UI;

using Microsoft.Xna.Framework;

namespace CruZ.Framework.GameSystem.ECS
{
    internal class ECSManager : IDisposable
    {
        private ECSManager() 
        {
            _world = new World();
            _world.
                AddSystem(new RenderSystem()).
                AddSystem(new AnimationSystem()).
                AddSystem(new ScriptSystem()).
                AddSystem(UISystem.CreateContext());
        }

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
            if(_instance != null && !_instance._isDisposed)
                throw new InvalidOperationException("Require dispose");
            return _instance = new ECSManager();
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
            if(!_isDisposed)
            {
                _isDisposed = true;
                _world.Dispose();
            }
            
        }

        bool _isDisposed = false;
    }
}