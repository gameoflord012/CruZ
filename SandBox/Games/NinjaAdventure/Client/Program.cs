using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using CruZ.GameEngine;

namespace NinjaAdventure.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            GameWrapper game = new();
            GameApplication.CreateContext(game, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resource"));

            GameApplication.Initialized += GameApplication_Initialized;
            game.Run();
            GameApplication.Initialized -= GameApplication_Initialized;
        }

        private static void GameApplication_Initialized()
        {
            Client.CreateContext();

            _scene = new NinjaAdventureScene();
            _scene.MonsterSpawner.SpawnDuration = -1;

            _gameState = new(_scene);
            _charCreation = new();

            GameApplication.MainCamera.Zoom = 60f;

            Task.Factory.StartNew(() =>
            {
                _charCreation.Communicate();

                while(true)
                {
                    // constantly update game state
                    _gameState.Communicate();
                }
            });
        }

        private static NinjaAdventureScene _scene;
        private static GameStateCommunicator _gameState;
        private static CharacterCreationCommunicator _charCreation;
    }
}
