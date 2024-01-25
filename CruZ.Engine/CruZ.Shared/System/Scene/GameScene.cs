using CruZ.Components;
using CruZ.Resource;
using CruZ.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace CruZ
{
    public partial class GameScene : IHasResourcePath
    {
        public event Action<TransformEntity>? OnEntityAdded;
        public event Action<TransformEntity>? OnEntityRemoved;

        public string               Name = "";

        [JsonIgnore]
        public TransformEntity[]    Entities    { get => _entities.ToArray(); }
        public string               ResourcePath { get; set; }

        public void AddEntity(TransformEntity e)
        {
            if (_entities.Contains(e)) return;
            _entities.Add(e);
            e.IsActive = _isActive;

            OnEntityAdded?.Invoke(e);
        }

        public void RemoveFromScene(TransformEntity e)
        {
            if (_entities.Contains(e)) return;
            _entities.Remove(e);
            e.IsActive = _isActive;

            OnEntityRemoved?.Invoke(e);
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

        bool _isActive = false;

        [JsonProperty]
        List<TransformEntity>   _entities = [];
    }
}
