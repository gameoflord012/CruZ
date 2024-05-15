using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.ECS;
using CruZ.GameEngine.GameSystem.Scene;

using NinjaAdventure.Server;

namespace NinjaAdventure
{
    [SceneFactoryClass]
    public class NinjaScene
    {
        [SceneFactoryMethod]
        public static GameScene DemoMonsterSpawner()
        {
            return new MonsterSpawnerScene();
        }

        SpriteRendererComponent _spriteRenderer;
    }
}
