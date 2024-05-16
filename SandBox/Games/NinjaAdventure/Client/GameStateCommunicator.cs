using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using CruZ.GameEngine;

using NinjaAdventure.Packet;

namespace NinjaAdventure.Client
{
    internal class GameStateCommunicator
    {
        public GameStateCommunicator(MonsterSpawnerScene monsterSpawnerScene)
        {
            _monsterIdMapper = [];
            _monsterSpawner = monsterSpawnerScene.MonsterSpawner;
        }

        public byte[] GetRequest()
        {
            return Encoding.ASCII.GetBytes("GET_GAME_STATE");
        }

        public void ProcessResponse(byte[] bytes)
        {
            var sGameState = TranslateServerResponse(bytes);

            GameApplication.MarshalInvoke(() =>
            {
                HashSet<int> sMonsterIds = [];

                // spawn new server monster 
                foreach(var monsterData in sGameState.MonsterDatas)
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

        private GameState TranslateServerResponse(byte[] bytes)
        {
            DataContractSerializer serializer = new(typeof(GameState));

            using(MemoryStream stream = new(bytes))
            {
                return (GameState)serializer.ReadObject(stream)!;
            }
        }

        private MonsterSpawner _monsterSpawner;
        private Dictionary<int, int> _monsterIdMapper;
    }
}
