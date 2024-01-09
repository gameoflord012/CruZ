using CruZ.Components;
using CruZ.Resource;
using CruZ.Scene;
using CruZ.Systems;
using CruZ.Utility;
using System.Linq;
namespace CruZ.Games.AnimalGang
{
    class MyGame : GameApplication
    {
        protected override void Initialize()
        {
            base.Initialize();

            var scene = SceneManager.SceneAssets.Values.First();
            ResourceManager.InitResource("scenes\\scene1.scene", scene, true);
            scene.Dispose();

            scene = ResourceManager.LoadResource<GameScene>("scenes\\scene1.scene");
            scene.SetActive(true);
        }

        protected override void EndRun()
        {
            base.EndRun();

            Logging.FlushToDebug();
        }

        public static void Main(string[] args)
        {
            new MyGame();
        }
    }
}