using CruZ.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;

namespace CruZ.Systems
{
    public partial class ECS
    {
        private ECS(IECSContextProvider contextProvider)
        {
            contextProvider.ECSDraw += Draw;
            contextProvider.ECSUpdate += Update;
            contextProvider.InitializeECSSystem += InitializeSystem;
        }

        private void InitializeSystem()
        {
            _world = new WorldBuilder().
                AddSystem(new EntityEventSystem()).
                AddSystem(new SpriteSystem()).
                AddSystem(new AnimatedSystem()).
                AddSystem(new PhysicSystem()).
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