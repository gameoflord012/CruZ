using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.ECS;
using CruZ.GameEngine.GameSystem.Scene;

namespace NinjaAdventure
{
    [SceneFactoryClass]
    public class NinjaScene
    {
        [SceneFactoryMethod]
        GameScene DemoNinjaScene()
        {
            var scene = new GameScene();
            {
                var ninja = new NinjaCharacter(scene);
                var monsterSpawner = new MonsterSpawner(scene, ninja.Entity.Transform);
            }
            return scene;
        }

        SpriteRendererComponent _spriteRenderer;
    }
}
