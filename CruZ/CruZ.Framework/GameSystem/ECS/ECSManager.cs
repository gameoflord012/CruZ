using System;
using System.Diagnostics;

using CruZ.Common.ECS;
using CruZ.Framework.GameSystem.Animation;
using CruZ.Framework.GameSystem.Render;
using CruZ.Framework.GameSystem.Script;
using CruZ.Framework.UI;

using Microsoft.Xna.Framework;

namespace CruZ.Framework.GameSystem.ECS
{
    interface IECSController
    {
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
        void Initialize();
    }

    public class ECSManager : IECSController, IDisposable
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

        void IECSController.Initialize()
        {
            _world.Initialize();
        }

        void IECSController.Update(GameTime gameTime)
        {
            _world.SystemsUpdate(gameTime);
        }

        void IECSController.Draw(GameTime gameTime)
        {
            _world.SystemsDraw(gameTime);
        }

        World _world;

        internal static IECSController CreateContext()
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