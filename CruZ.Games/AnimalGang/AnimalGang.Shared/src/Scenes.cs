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
            var player_sp = player.GetComponent<SpriteComponent>();
            player_sp.SortingLayer = 1;
            player_sp.YLayerDepth = true;

            var dungeonFloor = ECS.CreateEntity();
            dungeonFloor.Name = "dungeonFloor";
            dungeonFloor.AddComponent(new SpriteComponent());
            dungeonFloor.AddComponent(new TileComponent());
            var dungeonFloor_sp = dungeonFloor.GetComponent<SpriteComponent>();
            dungeonFloor_sp.SortingLayer = 0;
            dungeonFloor_sp.LoadTexture("tiles\\dungeon-floor");

            var dungeonWall = ECS.CreateEntity();
            dungeonWall.AddComponent(new SpriteComponent());
            dungeonWall.AddComponent(new TileComponent());
            var dungeonWall_sp = dungeonWall.GetComponent<SpriteComponent>();
            dungeonWall_sp.LoadTexture("tiles\\dungeon-wall");
            dungeonWall_sp.SortingLayer = 1;
            dungeonWall_sp.YLayerDepth = true;

            scene.AddEntity(dungeonFloor);
            scene.AddEntity(dungeonWall);
            scene.AddEntity(player);

            return scene;
        }
    }
}