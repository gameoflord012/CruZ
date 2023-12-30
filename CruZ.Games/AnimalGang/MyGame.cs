using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;

namespace CruZ.Game
{
    class MyGame : GameApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            var im = Content.Load<Texture2D>("image");

            var e1 = ECS.CreateEntity();
            e1.AddComponent(im);

            CreateScene();
            _scene.AddEntity(e1);
        }

        public void CreateScene()
        {
            _scene = new();
            SceneManager.LoadScene(_scene);
        }

        GameScene _scene;

        public static void Main(string[] args)
        {
            new MyGame();
        }
    }
}