using System;
using System.Collections.Generic;
using System.Diagnostics;

using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.ECS;
using CruZ.GameEngine.GameSystem.Scene;
using CruZ.GameEngine.Utility;

using Microsoft.Xna.Framework;

namespace NinjaAdventure
{
    internal class MonsterSpawner : ScriptingEntity
    {
        public float SpawnRadius = 5;
        public float SpawnDuration = 1f;

        public MonsterSpawner(GameScene gameScene) : base(gameScene)
        {
            Entity.Name = "MonsterSpawner";

            _gameScene = gameScene;
            _monsterPool = [];
            _spawnTimer = new();
            _random = new();

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

        protected override void OnUpdating(GameTime time)
        {
            if(_spawnTimer.GetElapsed() > SpawnDuration)
            {
                _spawnTimer.Restart();

                // spawn logic
                var r = SpawnRadius * float.Sqrt(_random.NextSingle());
                var theta = _random.NextSingle() * 2f * MathF.PI;

                var spawnX = Entity.Position.X + r * MathF.Cos(theta);
                var spawnY = Entity.Position.Y + r * MathF.Sin(theta);

                var monster = PopMonsterFromPool();
                monster.Reset(new Vector2(spawnX, spawnY), null);
            }
        }

        private LarvaMonster PopMonsterFromPool()
        {
            if (_monsterPool.Count == 0)
            {
                var larva = new LarvaMonster(_gameScene, _monsterRenderer);
                larva.PoolReturn += Larva_PoolReturn;
                _monsterPool.Push(larva);
            }

            return _monsterPool.Pop();
        }

        private void Larva_PoolReturn(LarvaMonster monster)
        {
            monster.PoolReturn -= Larva_PoolReturn;
            _monsterPool.Push(monster);
        }

        Pool<LarvaMonster> _monsterPool;
        SpriteRendererComponent _monsterRenderer;

        Random _random;
        Stopwatch _spawnTimer;
        GameScene _gameScene;
    }
}
