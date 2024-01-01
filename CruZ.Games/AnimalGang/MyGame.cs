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

            _charTemplate = new MainCharacter();
            ECS.BuildTemplate(_charTemplate);
        }

        MainCharacter _charTemplate;

        public static void Main(string[] args)
        {
            new MyGame();
        }
    }
}