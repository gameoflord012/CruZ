using System.Collections.Generic;
using System.Linq;

using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem.Scene;

using Microsoft.Xna.Framework;

namespace NinjaAdventure
{
    internal class MonsterSpawnerScene : GameScene
    {
        public MonsterSpawnerScene()
        {
            GameApplication.MainCamera.Zoom = 60f;
            _monsterSpawner = new MonsterSpawner(this, null);
        }

        public IEnumerable<Vector2> MonsterPositions
        {
            get => _monsterSpawner.AliveMonsters.Select(e => e.Postition);
        }

        private MonsterSpawner _monsterSpawner;
    }
}
