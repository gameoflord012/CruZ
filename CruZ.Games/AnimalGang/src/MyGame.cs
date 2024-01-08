using CruZ.Components;
using CruZ.Resource;
using CruZ.Systems;
namespace CruZ.Games.AnimalGang
{
    class MyGame : GameApplication
    {
        protected override void Initialize()
        {
            base.Initialize();

            var scene = new GameScene();

            var e = ECS.CreateEntity();

            var anim = new AnimationComponent();
            anim.LoadSpriteSheet("anims/player-walk.sf");

            e.AddComponent(new SpriteComponent());
            e.AddComponent(new MainCharacter());
            e.AddComponent(anim);

            scene.AddEntity(e);
            ResourceManager.CreateResource("scenes\\scene1.scene", scene);
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