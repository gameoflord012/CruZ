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

            var dungegonFloor = ECS.CreateEntity();
            dungegonFloor.Name = "dungeonFloor";
            dungegonFloor.AddComponent(new SpriteComponent());
            dungegonFloor.AddComponent(new TileComponent());
            var dungeonFloor_sp = dungegonFloor.GetComponent<SpriteComponent>();
            dungeonFloor_sp.LayerDepth = 0.1f;
            dungeonFloor_sp.LoadTexture("tiles\\dungeon-floor");

            var dungeonWall = ECS.CreateEntity();
            dungeonWall.AddComponent(new SpriteComponent());
            dungeonWall.AddComponent(new TileComponent());
            var dungeonWall_sp = dungeonWall.GetComponent<SpriteComponent>();
            dungeonWall_sp.LoadTexture("tiles\\dungeon-wall");

            scene.AddToScene(dungegonFloor);
            scene.AddToScene(dungeonWall);

            return scene;
        }
    }
}