﻿//using System.Linq;

//using CruZ.Common;
//namespace CruZ.Game.AnimalGang
//{
//    class MyGame : GameApplication
//    {
//        protected override void OnInitialize()
//        {
//            base.OnInitialize();

//            var scene = SceneManager.SceneAssets.Values.First();
//            ResourceManager.CreateResource("scenes\\scene1.scene", scene, true);
//            scene.Dispose();

//            scene = ResourceManager.LoadResource<GameScene>("scenes\\scene1.scene");
//            scene.SetActive(true);
//        }

//        protected override void OnDraw(GameTime gameTime)
//        {
//            base.OnDraw(gameTime);
//        }

//        protected override void OnEndRun()
//        {
//            base.OnEndRun();

//            //LogService.FlushToDebug();
//        }

//        public static void Main(string[] args)
//        {
//            new MyGame();
//        }
//    }
//}