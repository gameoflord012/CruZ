﻿using CruZ.Components;
using CruZ.Resource;
namespace CruZ.Game
{
    class MyGame : GameApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            var scene = new GameScene();
            var e = ECS.CreateEntity();
            e.AddComponent(new MainCharacter());
            scene.AddEntity(e);
            ResourceManager.CreateResource("scene1.uri", scene, true);
            e.RemoveFromWorld();

            scene = ResourceManager.LoadResource<GameScene>("scene1.uri");
            SceneManager.SetActive(scene);
        }

        MainCharacter _charTemplate;

        public static void Main(string[] args)
        {
            new MyGame();
        }
    }
}