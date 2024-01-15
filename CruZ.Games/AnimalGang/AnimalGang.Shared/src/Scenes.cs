using CruZ.Components;
using CruZ.Games.AnimalGang;
using CruZ.Systems;

namespace CruZ.Scene
{
    [SceneAssetClass("Scenes")]
    public class Scenes
    {
        [SceneAssetMethod("Scene1")]
        public static GameScene Scene1()
        {
            var scene = new GameScene();

            var e = ECS.CreateEntity();

            var anim = new AnimationComponent();
            anim.LoadSpriteSheet("anims/player-walk.sf",            "player-normal");
            anim.LoadSpriteSheet("anims/player-sword-idle.sf",      "player-sword-idle");
            anim.LoadSpriteSheet("anims/player-sword-attack.sf",    "player-sword-attack");

            e.AddComponent(new SpriteComponent());
            e.AddComponent(new MainCharacter());
            e.AddComponent(anim);

            e.Transform.Scale = new(2, 2);

            scene.AddToScene(e);

            return scene;
        }
    }
}