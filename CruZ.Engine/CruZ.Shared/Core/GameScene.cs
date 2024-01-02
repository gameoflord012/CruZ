using CruZ.Components;
using CruZ.Resource;
using CruZ.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace CruZ
{
    public class GameScene
    {
        public event Action<TransformEntity>? OnEntityAdded;
        public event Action<TransformEntity>? OnEntityRemoved;

        public string               Name = "";

        [JsonIgnore]
        public TransformEntity[]    Entities    { get => _entities.ToArray(); }

        public void AddEntity(TransformEntity e)
        {
            if (_entities.Contains(e)) return;
            _entities.Add(e);
            OnEntityAdded?.Invoke(e);
        }

        public void RemoveEntity(TransformEntity e)
        {
            if (_entities.Contains(e)) return;
            _entities.Remove(e);
            OnEntityRemoved?.Invoke(e);
        }

        [JsonProperty]
        List<TransformEntity>   _entities = [];
    }
}
