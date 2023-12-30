using CruZ.Systems;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;

namespace CruZ
{
    class ECS
    {
        public ECS(MGWrapper core)
        {
            core.OnInitialize += Initialize;
            core.OnUpdate += Update;
            core.OnDraw += Draw;
        }

        private void Initialize()
        {
            _world = new WorldBuilder().
                AddSystem(new SpriteSystem()).
                AddSystem(new AnimatedSystem()).
                AddSystem(new PhysicSystem()).
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

        public World World { get => _world; }
    }
}