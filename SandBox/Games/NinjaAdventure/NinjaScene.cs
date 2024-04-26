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
                TransformEntity entityRoot = scene.CreateEntity("World");

                _spriteRenderer = new SpriteRendererComponent();
                {

                }
                entityRoot.AddComponent(_spriteRenderer);

                var ninja = new NinjaCharacter(scene, _spriteRenderer);
                ninja.Entity.Parent = entityRoot;

                var larva = new LarvaMonster(scene, _spriteRenderer);
                larva.Entity.Parent = entityRoot;
                larva.Follow = ninja.Entity.Transform;
            }
            return scene;
        }

        SpriteRendererComponent _spriteRenderer;
    }
}
