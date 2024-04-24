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

                new NinjaCharacter(scene, _spriteRenderer).Entity.Parent = entityRoot;
            }
            return scene;
        }

        SpriteRendererComponent _spriteRenderer;
    }
}
