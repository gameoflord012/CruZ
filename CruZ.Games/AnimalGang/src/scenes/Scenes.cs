using CruZ.Components;
using CruZ.Games.AnimalGang;
using CruZ.Systems;

namespace CruZ.Scene
{
    [SceneAssetClass("Class")]
    public class Scenes
    {
        [SceneAssetMethod("method")]
        public static GameScene Scene1()
        {
            var scene = new GameScene();

            var e = ECS.CreateEntity();

            var anim = new AnimationComponent();
            anim.LoadSpriteSheet("anims/player-walk.sf");

            e.AddComponent(new SpriteComponent());
            e.AddComponent(new MainCharacter());
            e.AddComponent(anim);

            scene.AddToScene(e);

            return scene;
        }
    }
}