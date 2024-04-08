using System;

using CruZ.Common.ECS;
using CruZ.Framework.GameSystem.Animation;
using CruZ.Framework.GameSystem.Render;
using CruZ.Framework.GameSystem.Script;

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
                AddSystem(new ScriptSystem());
        }

        void IECSController.Initialize()
        {
            _world.Initialize();
        }

        void IECSController.Update(GameTime gameTime)
        {
            _world.Update(gameTime);
        }

        void IECSController.Draw(GameTime gameTime)
        {
            _world.Draw(gameTime);
        }

        World _world;

        internal static IECSController CreateContext()
        {
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
            _world.Dispose();
        }
    }
}