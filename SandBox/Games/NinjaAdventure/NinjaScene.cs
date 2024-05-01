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

                _larva = new LarvaMonster(scene, _spriteRenderer);
                _larva.Entity.Parent = entityRoot;
                _larva.Follow = ninja.Entity.Transform;
                _larva.BecomeUseless += Larva_BecomeUseless;
            }
            return scene;
        }

        private void Larva_BecomeUseless(LarvaMonster useless)
        {
            useless.Dispose();
            _larva = null;
        }

        SpriteRendererComponent _spriteRenderer;
        private LarvaMonster? _larva;
    }
}
