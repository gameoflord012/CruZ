using CruZ.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using System.Collections.Generic;

namespace CruZ.Systems
{
    public partial class ECS
    {
        private ECS()
        {
            Core.OnUpdate += Update;
            Core.OnDraw += Draw;

            _world = new WorldBuilder().
                AddSystem(new EntityEventSystem()).
                AddSystem(new SpriteSystem()).
                AddSystem(new AnimatedSystem()).
                AddSystem(new PhysicSystem()).
                AddSystem(new EntityScriptSystem()).
                Build();

            //_entityBuilder = new(_world);
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