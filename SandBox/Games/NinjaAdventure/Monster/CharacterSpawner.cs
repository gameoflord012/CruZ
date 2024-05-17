using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.ECS;
using CruZ.GameEngine.GameSystem.Scene;
using CruZ.GameEngine.GameSystem.Script;
using CruZ.GameEngine.Utility;

using Microsoft.Xna.Framework;

using NinjaAdventure.Ultility;

namespace NinjaAdventure
{
    internal class CharacterSpawner : ScriptingEntity
    {
        private const int MaxPoolCount = 5;
        private const float SpawnRadius = 5;

        public CharacterSpawner(GameScene gameScene) : base(gameScene)
        {
            Entity.Name = "CharacterSpawner";
            SpawnDuration = -1;

            _gameScene = gameScene;
            _characterPool = new(() => new NinjaCharacter(gameScene));
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

        protected override void OnUpdating(ScriptUpdateArgs args)
        {
            if(SpawnDuration <= 0f)
            {
                return;
            }

            if(_characterPool.PopCount == MaxPoolCount)
            {
                // despawn if max
                Clear();
            }

            if(_spawnTimer.GetElapsed() > SpawnDuration &&
                (_characterPool.PopCount < MaxPoolCount))
            {
                _spawnTimer.Restart();

                // randomly spawn in a circle 
                Vector2 spawnPosition = _random.RandomPosition(SpawnRadius, Entity.Position);
                Spawn(spawnPosition);
            }
        }

        public NinjaCharacter Spawn(Vector2 position)
        {
            var character = _characterPool.Pop();
            character.Reset(position);
            return character;
        }

        public void Despawn(int id)
        {
            if(TryGet(id, out NinjaCharacter? character))
            {
                character.ReturnToPool();
            }
        }

        public void Clear()
        {
            foreach(var character in _characterPool.Pops)
            {
                character.ReturnToPool();
            }
        }

        public IReadOnlyCollection<NinjaCharacter> Alives
        {
            get => _characterPool.Pops;
        }

        public bool TryGet(int id, [NotNullWhen(true)] out NinjaCharacter? character)
        {
            character = Alives.Where(e => e.Id == id).FirstOrDefault();
            return character != null;
        }

        public bool Contains(int id)
        {
            return TryGet(id, out _);
        }

        public float SpawnDuration
        {
            get;
            set;
        }

        private Pool<NinjaCharacter> _characterPool;
        private SpriteRendererComponent _monsterRenderer;
        private Random _random;
        private Stopwatch _spawnTimer;
        private GameScene _gameScene;
    }
}
