using System;
using System.Diagnostics;

using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.ECS;
using CruZ.GameEngine.GameSystem.Scene;
using CruZ.GameEngine.GameSystem.Script;
using CruZ.GameEngine.GameSystem.UI;
using CruZ.GameEngine.Utility;

using Microsoft.Xna.Framework;

namespace NinjaAdventure
{
    internal class MonsterSpawner : ScriptingEntity
    {
        private const int MaxPoolCount = 5;
        private const float SpawnRadius = 5;
        private const float SpawnDuration = 1f;

        public MonsterSpawner(GameScene gameScene, Transform follow) : base(gameScene)
        {
            Entity.Name = "MonsterSpawner";

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
            if(_spawnTimer.GetElapsed() > SpawnDuration &&
                (_monsterPool.PopCount < MaxPoolCount))
            {
                _spawnTimer.Restart();

                // circle distribution
                var r = SpawnRadius * float.Sqrt(_random.NextSingle());
                var theta = _random.NextSingle() * 2f * MathF.PI;

                var spawnX = Entity.Position.X + r * MathF.Cos(theta);
                var spawnY = Entity.Position.Y + r * MathF.Sin(theta);

                var monster = _monsterPool.Pop();
                monster.Reset(new Vector2(spawnX, spawnY), _follow);
            }
        }

        private Pool<LarvaMonster> _monsterPool;
        private SpriteRendererComponent _monsterRenderer;
        private Random _random;
        private Stopwatch _spawnTimer;
        private GameScene _gameScene;
        private Transform _follow;
    }
}
