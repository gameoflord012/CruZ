using System;
using System.Collections.Generic;
using System.Linq;

using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem.Scene;

using NinjaAdventure.Packet;
using NinjaAdventure.Ultility;

namespace NinjaAdventure.Editor
{
    [GameSceneDecorator]
    internal class NinjaAdventureEditorScene : GameSceneDecorator
    {
        public NinjaAdventureEditorScene(GameScene gameScene) : base(gameScene)
        {
        }

        protected override void OnInitialize()
        {
            GameApplication.MainCamera.Zoom = 60f;

            _characterSpawner = new CharacterSpawner(GameScene);
            _monsterSpawner = new MonsterSpawner(GameScene, null);
            _random = new();
        }

        public NinjaCharacter CreateCharacter()
        {
            var spawnPosition = _random.RandomPosition(5, RootEntity.Position);
            return _characterSpawner.Spawn(spawnPosition);
        }

        public IEnumerable<MonsterData> Monsters
        {
            get => _monsterSpawner.Alives.Select(e => new MonsterData(e));
        }

        public IEnumerable<CharacterData> Characters
        {
            get => _characterSpawner.Alives.Select(e => new CharacterData(e));
        }

        public MonsterSpawner MonsterSpawner
        {
            get => _monsterSpawner;
        }

        private CharacterSpawner _characterSpawner;
        private MonsterSpawner _monsterSpawner;
        private Random _random;
    }
}
