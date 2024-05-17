using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.ECS;
using CruZ.GameEngine.GameSystem.Scene;
using CruZ.GameEngine.GameSystem.Script;
using CruZ.GameEngine.GameSystem.UI;
using CruZ.GameEngine.Utility;

using Microsoft.Xna.Framework;

using NinjaAdventure.Ultility;

namespace NinjaAdventure
{
    internal class MonsterSpawner : ScriptingEntity
    {
        private const int MaxPoolCount = 5;
        private const float SpawnRadius = 5;

        public MonsterSpawner(GameScene gameScene, Transform? follow) : base(gameScene)
        {
            Entity.Name = "MonsterSpawner";
            SpawnDuration = 1;

            _gameScene = gameScene;
            _monsterPool = new(() => new LarvaMonster(gameScene, _monsterRenderer!));
            _spawnTimer = new();
            _random = new();
            _follow = follow;

            InitializeComponents();

            _spawnTimer.Start();
        }

        private void InitializeComponents()
        {
            _monsterRenderer = new SpriteRendererComponent();
            {

            }
            Entity.AddComponent(_monsterRenderer);
        }

        protected override void OnUpdating(ScriptUpdateArgs args)
        {
            if(SpawnDuration <= 0f)
            {
                return;
            }

            if(_monsterPool.PopCount == MaxPoolCount)
            {
                // despawn if max
                Clear();
            }

            if(_spawnTimer.GetElapsed() > SpawnDuration &&
                (_monsterPool.PopCount < MaxPoolCount))
            {
                _spawnTimer.Restart();

                // randomly spawn in a circle 
                Vector2 spawnPosition = _random.RandomPosition(SpawnRadius, Entity.Position);
                SpawnMonster(spawnPosition.X, spawnPosition.Y);
            }
        }

        public LarvaMonster SpawnMonster(float spawnX, float spawnY)
        {
            var monster = _monsterPool.Pop();
            monster.Reset(new Vector2(spawnX, spawnY), _follow);
            return monster;
        }

        public void DespawnMonster(int monsterId)
        {
            if(TryGet(monsterId, out LarvaMonster? larvaMonster))
            {
                larvaMonster.ReturnToPool();
            }
        }

        public void Clear()
        {
            foreach(var monster in _monsterPool.Pops)
            {
                monster.ReturnToPool();
            }
        }

        public IReadOnlyCollection<LarvaMonster> Alives
        {
            get => _monsterPool.Pops;
        }

        public bool TryGet(int monsterId, [NotNullWhen(true)] out LarvaMonster? larvaMonster)
        {
            larvaMonster = Alives.Where(e => e.Id == monsterId).FirstOrDefault();
            return larvaMonster != null;
        }

        public bool Contains(int monsterId)
        {
            return TryGet(monsterId, out _);
        }

        public float SpawnDuration
        {
            get;
            set;
        }

        private Pool<LarvaMonster> _monsterPool;
        private SpriteRendererComponent _monsterRenderer;
        private Random _random;
        private Stopwatch _spawnTimer;
        private GameScene _gameScene;
        private Transform? _follow;
    }
}
