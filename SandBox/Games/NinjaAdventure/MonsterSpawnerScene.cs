using System.Collections.Generic;
using System.Linq;

using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem.Scene;

using NinjaAdventure.Packet;

namespace NinjaAdventure
{
    internal class MonsterSpawnerScene : GameScene
    {
        public MonsterSpawnerScene()
        {
            GameApplication.MainCamera.Zoom = 60f;
            _monsterSpawner = new MonsterSpawner(this, null);
        }

        public IEnumerable<MonsterData> MonsterDatas
        {
            get => _monsterSpawner.AliveMonsters.Select(e => new MonsterData(e.Postition, e.Id));
        }

        public float SpawnDuration
        {
            get => _monsterSpawner.SpawnDuration;
            set => _monsterSpawner.SpawnDuration = value;
        }

        public MonsterSpawner MonsterSpawner
        {
            get => _monsterSpawner;
        }

        private MonsterSpawner _monsterSpawner;
    }
}
