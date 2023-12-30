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

            var e = ECS.CreateEntity();
            foreach(var c in _char.InitialComponents())
            {
                e.AddComponent(c, c.GetType());
            }

            _char.ApplyTo(e);
            _scene.AddEntity(e);
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