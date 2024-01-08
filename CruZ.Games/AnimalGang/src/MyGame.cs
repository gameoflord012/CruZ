using CruZ.Components;
using CruZ.Resource;
using CruZ.Scene;
using CruZ.Systems;
using System.Linq;
namespace CruZ.Games.AnimalGang
{
    class MyGame : GameApplication
    {
        protected override void Initialize()
        {
            base.Initialize();

            var scene = SceneManager.SceneAssets.Values.First();
            
            ResourceManager.CreateResource("scenes\\scene1.scene", scene);

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