using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem.Scene;

using NinjaAdventure;
using NinjaAdventure.Packet;

class Program
{
    private static readonly IPAddress BroadCastAddress = IPAddress.Parse("192.168.1.255");
    private static int Port = 11000;

    static void Main(string[] args)
    {
        GameWrapper game = new();
        _gameApp = GameApplication.CreateContext(game, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resource"));

        GameApplication.Initialized += GameApplication_Initialized;
        game.Run();
        GameApplication.Initialized -= GameApplication_Initialized;


        //UdpClient udpClient = new();

        //// send request
        //IPEndPoint broadCast = new(BroadCastAddress, Port);
        //udpClient.Send(Encoding.ASCII.GetBytes("GET_GAME_STATE"), broadCast);

        //// receive request
        //IPEndPoint serverEP = new(IPAddress.Any, Port);
        //var bytes = udpClient.Receive(ref serverEP);

        //ProcessServerResponse(bytes);
    }

    private static void GameApplication_Initialized()
    {
        _gameScene = new GameScene();
        _monsterSpawner = new MonsterSpawner(_gameScene, null);
        _monsterSpawner.SpawnDuration = -1;
        _monsterIdMapper = [];
        GameApplication.MainCamera.Zoom = 60f;

        UdpClient udpClient = new();
        IPEndPoint broadCast = new(BroadCastAddress, Port);

        Task.Factory.StartNew(() =>
        {
            while(true)
            {
                udpClient.Send(Encoding.ASCII.GetBytes("GET_GAME_STATE"), broadCast);
                IPEndPoint serverEP = new(IPAddress.Any, Port);
                var bytes = udpClient.Receive(ref serverEP);

                var serverGameState = ProcessServerResponse(bytes);

                _gameApp.MarshalInvoke(() =>
                {
                    HashSet<int> sMonsterIds = [];

                    // spawn new server monster 
                    foreach(var monsterData in serverGameState.MonsterDatas)
                    {
                        if(!_monsterIdMapper.ContainsKey(monsterData.Id))
                        {
                            var spawnedMonster = _monsterSpawner.SpawnMonster(monsterData.Position.X, monsterData.Position.Y);
                            _monsterIdMapper[monsterData.Id] = spawnedMonster.Id;
                        }

                        if(_monsterSpawner.TryGet(_monsterIdMapper[monsterData.Id], out LarvaMonster? monster))
                        {
                            monster.Postition = monsterData.Position;
                        }

                        sMonsterIds.Add(monsterData.Id);
                    }

                    // despawn monster that's not exists in the server
                    foreach(var key in _monsterIdMapper.Keys.ToImmutableArray())
                    {
                        if(!sMonsterIds.Contains(key))
                        {
                            _monsterSpawner.DespawnMonster(_monsterIdMapper[key]);
                            _monsterIdMapper.Remove(key);

                            Console.WriteLine("Despawned");
                        }
                    }
                });
            }
        });

    }

    private static GameState ProcessServerResponse(byte[] bytes)
    {
        DataContractSerializer serializer = new(typeof(GameState));

        using(MemoryStream stream = new(bytes))
        {
            return (GameState)serializer.ReadObject(stream)!;
        }
    }

    static GameScene _gameScene;
    static GameApplication _gameApp;
    static MonsterSpawner _monsterSpawner;
    static Dictionary<int, int> _monsterIdMapper;
}
