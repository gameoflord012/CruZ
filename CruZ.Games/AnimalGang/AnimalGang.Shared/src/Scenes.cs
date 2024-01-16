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
            e.GetComponent<SpriteComponent>().LayerDepth = 0.2f;
            e.AddComponent(anim);
            e.Transform.Scale = new(5, 5);

            var backGround = ECS.CreateEntity();
            backGround.AddComponent(new SpriteComponent());
            var sp = backGround.GetComponent<SpriteComponent>();
            sp.LayerDepth = 0.1f;
            sp.LoadTexture("tiles\\tile.png");

            scene.AddToScene(e);
            scene.AddToScene(backGround);

            return scene;
        }
    }
}