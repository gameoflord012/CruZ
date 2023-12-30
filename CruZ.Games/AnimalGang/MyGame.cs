using CruZ.Components;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;

namespace CruZ.Game
{
    class MyGame : GameApplication
    {
        public override void Initialize()
        {
            base.Initialize();
            CreateScene();

            _char = new MainCharacter();
            _char.Apply(ECS.CreateEntity());

            _scene.AddEntity(_char.AppliedEntity);
        }

        public void CreateScene()
        {
            _scene = new();
            SceneManager.LoadScene(_scene);
        }

        GameScene _scene;
        MainCharacter _char;

        public static void Main(string[] args)
        {
            new MyGame();
        }
    }
}