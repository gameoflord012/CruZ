﻿using CruZ.Components;
using CruZ.Resource;
namespace CruZ.Games.AnimalGang
{
    class MyGame : GameApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            var scene = new GameScene();
            var e = ECS.CreateEntity();

            e.AddComponent(new SpriteComponent());
            e.AddComponent(new MainCharacter());
            e.AddComponent(new AnimationComponent());

            scene.AddEntity(e);
            ResourceManager.CreateResource("scenes\\scene1.scene", scene, true);
            e.RemoveFromWorld();

            scene = ResourceManager.LoadResource<GameScene>("scenes\\scene1.scene");
            SceneManager.SetActive(scene);
        }

        MainCharacter _charTemplate;

        public static void Main(string[] args)
        {
            new MyGame();
        }
    }
}