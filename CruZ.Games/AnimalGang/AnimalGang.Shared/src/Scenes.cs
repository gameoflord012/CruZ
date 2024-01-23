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

            var player = ECS.CreateEntity();
            player.Name = "Player";

            var anim = new AnimationComponent();
            anim.LoadSpriteSheet("anims/player-walk.sf",            "player-normal");
            anim.LoadSpriteSheet("anims/player-sword-idle.sf",      "player-sword-idle");
            anim.LoadSpriteSheet("anims/player-sword-attack.sf",    "player-sword-attack");

            player.AddComponent(new SpriteComponent());
            player.AddComponent(new MainCharacter());
            player.GetComponent<SpriteComponent>().LayerDepth = 0.2f;
            player.AddComponent(anim);
            player.Transform.Scale = new(0.1f, 0.1f);

            var backGround = ECS.CreateEntity();
            backGround.Name = "Background";
            backGround.AddComponent(new SpriteComponent());
            backGround.AddComponent(new TileComponent());
            var sp = backGround.GetComponent<SpriteComponent>();
            sp.LayerDepth = 0.1f;
            sp.LoadTexture("tiles\\tile.png");
            backGround.Transform.Scale = new(0.1f, 0.1f);

            //scene.AddToScene(player);
            scene.AddToScene(backGround);

            return scene;
        }
    }
}