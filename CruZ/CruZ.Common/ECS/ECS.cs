using Microsoft.Xna.Framework;

using MonoGame.Extended.Entities;

namespace CruZ.Common.ECS
{
    public partial class ECSManager
    {
        private ECSManager(IECSContextProvider contextProvider)
        {
            contextProvider.ECSDraw += Draw;
            contextProvider.ECSUpdate += Update;
            contextProvider.InitializeECSSystem += InitializeSystem;
        }

        private void InitializeSystem()
        {
            _world = new WorldBuilder().
                //AddSystem(new EntityEventSystem()).
                AddSystem(new RenderSystem()).
                AddSystem(new AnimatedSystem()).
                //AddSystem(new PhysicSystem()).
                AddSystem(new EntityScriptSystem()).
                Build();
        }

        private void Update(GameTime gameTime)
        {
            _world.Update(gameTime);
        }

        private void Draw(GameTime gameTime)
        {
            _world.Draw(gameTime);
        }

        World _world;
    }
}