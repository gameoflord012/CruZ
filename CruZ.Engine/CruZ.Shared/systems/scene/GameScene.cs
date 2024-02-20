using CruZ.Components;
using CruZ.Resource;
using CruZ.Serialization;
using CruZ.Systems;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace CruZ
{
    public partial class GameScene : IHostResource, ICustomSerializable
    {
        public event Action<TransformEntity>? EntityAdded;
        public event Action<TransformEntity>? EntityRemoved;

        public string               Name = "";

        [JsonIgnore]
        public TransformEntity[]        Entities        { get => _entities.ToArray(); }
        public ResourceInfo?            ResourceInfo    { get; set; }

        private GameScene(GameApplication gameApp) 
        {
            _gameApp = gameApp;
            _gameApp.ExitEvent += Game_Exit;
        }

        public void AddEntity(TransformEntity e)
        {
            if (_entities.Contains(e)) return;
            _entities.Add(e);
            e.IsActive = _isActive;

            EntityAdded?.Invoke(e);
        }

        public void RemoveEntity(TransformEntity e)
        {
            if (!_entities.Contains(e)) 
                throw new ArgumentException($"Entity \"{e}\" not in scene {this}");

            _entities.Remove(e);
            e.IsActive = _isActive;

            EntityRemoved?.Invoke(e);
        }

        public void SetActive(bool isActive)
        {
            if (_isActive == isActive) return;
            _isActive = isActive;

            foreach (var e in _entities)
            {
                e.IsActive = _isActive;
            }
        }

        public TransformEntity CreateEntity(string name = "New Entity")
        {
            var e = ECS.CreateEntity();
            e.Name = name;
            AddEntity(e);

            return e;
        }

        private void Game_Exit()
        {
            Dispose();
        }

        public ICustomSerializable? CreateDefault()
        {
            return GameApplication.CreateScene();
        }

        public void ReadJson(JsonReader reader, JsonSerializer serializer)
        {
            serializer.Populate(reader, this);
        }

        public void WriteJson(JsonWriter writer, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(nameof(Name));
            serializer.Serialize(writer, Name);

            writer.WritePropertyName(nameof(_entities));
            serializer.Serialize(writer, _entities);

            writer.WritePropertyName(nameof(ResourceInfo));
            serializer.Serialize(writer, ResourceInfo);

            writer.WriteEnd();
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name) ? ResourceInfo.ResourceName : Name;
        }

        bool _isActive = false;

        [JsonProperty]
        List<TransformEntity>   _entities = [];

        [JsonIgnore]
        GameApplication         _gameApp;
    }

    public partial class GameScene
    {
        public static GameScene Create(GameApplication gameApp)
        {
            return new(gameApp);
        }
    }
}
