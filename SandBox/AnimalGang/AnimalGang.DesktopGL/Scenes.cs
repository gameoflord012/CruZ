using CruZ.Common;
using CruZ.Common.ECS;
using CruZ.Common.Scene;

namespace AnimalGang
{
    [SceneAssetClass("Scenes")]
    public class Scenes
    {
        [SceneAssetMethod]
        public static GameScene Scene1()
        {
            var scene = new GameScene();

            var player = scene.CreateEntity();
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
            player_sp.SortByY = true;

            var dungeonFloor = scene.CreateEntity();
            dungeonFloor.Name = "dungeonFloor";
            dungeonFloor.AddComponent(new SpriteComponent());
            dungeonFloor.AddComponent(new TileComponent());
            var dungeonFloor_sp = dungeonFloor.GetComponent<SpriteComponent>();
            dungeonFloor_sp.SortingLayer = 0;
            dungeonFloor_sp.LoadTexture("tiles\\dungeon-floor");

            var dungeonWall = scene.CreateEntity();
            dungeonWall.Name = "dungeonWall";
            dungeonWall.AddComponent(new SpriteComponent());
            dungeonWall.AddComponent(new TileComponent());
            var dungeonWall_sp = dungeonWall.GetComponent<SpriteComponent>();
            dungeonWall_sp.LoadTexture("tiles\\dungeon-wall");
            dungeonWall_sp.SortingLayer = 1;
            dungeonWall_sp.SortByY = true;

            return scene;
        }

        [SceneAssetMethod]
        public static GameScene Scene2()
        {
            var scene = new GameScene();
            //
            // ground
            //
            var sp_ground = new SpriteComponent();
            sp_ground.LoadTexture("tiles\\tile3\\home-ground-behind.png");
            sp_ground.SortingLayer = 0;

            var ground = scene.CreateEntity("Ground");
            ground.AddComponent(sp_ground);
            ground.Scale = new(1f / 16f, 1f / 16f);
            // 
            // object
            //
            var sp_groundObj = new SpriteComponent();
            sp_groundObj.LoadTexture("tiles\\tile3\\home-object-mid.png");
            sp_groundObj.SortingLayer = 1;
            sp_groundObj.SortByY = true;

            var groundObj = scene.CreateEntity("Objects");
            groundObj.AddComponent(sp_groundObj);
            groundObj.Scale = new(1f / 16f, 1f / 16f);
            //
            // frontObj
            //
            var sp_frontObj = new SpriteComponent();
            sp_frontObj.LoadTexture("tiles\\tile3\\home-object-font.png");
            sp_frontObj.SortingLayer = 2;

            var frontObj = scene.CreateEntity("TopObj");
            frontObj.AddComponent(sp_frontObj);
            frontObj.Scale = new(1f / 16f, 1f / 16f);
            //
            // Player
            //
            var script_mainChar = new MainCharacter();
            
            var anims_mainChar = new AnimationComponent();
            anims_mainChar.LoadSpriteSheet("anims\\dark-ninja\\dark-ninja-walk.sf", "ninja-movement");

            var sp_mainChar = new SpriteComponent();
            sp_mainChar.SortingLayer = 1;
            sp_mainChar.SortByY = true;

            var mainChar = scene.CreateEntity("MainChar");
            mainChar.AddComponent(script_mainChar);
            mainChar.AddComponent(anims_mainChar);
            mainChar.AddComponent(sp_mainChar);
            //
            // Light
            //
            var light = scene.CreateEntity("Light");
            light.Scale = new(0.05f, 0.05f);
            var lightComp = new LightComponent();
            lightComp.SortingLayer = 1;
            light.AddComponent(lightComp);

            return scene;
        }
    }
}