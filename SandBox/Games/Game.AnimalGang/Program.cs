using CruZ.GameEngine;
using System;

namespace Game.AnimalGang
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            GameWrapper game = new();
            var app = GameApplication.CreateContext(game);
            GameApplication.Initialized += GameApplication_Initialized;
            app.Run();
        }

        private static void GameApplication_Initialized()
        {
            throw new NotImplementedException();
            //GameContext.GameResourceDir = "D:\\monogame-projects\\CruZ_GameEngine\\SandBox\\Games\\Game.AnimalGang\\Resource\\";
            //Camera.Main.Zoom = 0.25f;
            //Scenes.FlameScene().SetActive(true);
        }
    }
}
