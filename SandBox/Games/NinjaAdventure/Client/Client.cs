using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

using CruZ.GameEngine;

namespace NinjaAdventure.Client;

internal class Program
{
    private static readonly IPAddress BroadCastAddress = IPAddress.Parse("192.168.1.255");
    private static int Port = 11000;

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
        _monsterSpawnerScene = new MonsterSpawnerScene();
        _monsterSpawnerScene.SpawnDuration = -1;
        GameApplication.MainCamera.Zoom = 60f;
        _gameStateCommunicator = new(_monsterSpawnerScene);

        UdpClient udpClient = new();
        IPEndPoint broadCastEP = new(BroadCastAddress, Port);
        IPEndPoint serverEP = new(IPAddress.Any, Port);

        Task.Factory.StartNew(() =>
        {
            while(true)
            {
                udpClient.Send(_gameStateCommunicator.GetRequest(), broadCastEP);
                var bytes = udpClient.Receive(ref serverEP);

                _gameStateCommunicator.ProcessResponse(bytes);
            }
        });
    }

    private static MonsterSpawnerScene _monsterSpawnerScene;
    private static GameStateCommunicator _gameStateCommunicator;
}
