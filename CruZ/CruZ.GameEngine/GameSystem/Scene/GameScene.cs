using CruZ.GameEngine;
using CruZ.GameEngine.Serialization;
using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.Resource;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace CruZ.GameEngine.GameSystem.Scene
{
    public class GameScene : IResource, IDisposable, IJsonOnSerializing, IJsonOnDeserialized
    {
        public event Action<TransformEntity>? EntityAdded;
        public event Action<TransformEntity>? EntityRemoved;

        public string Name = ""; // temporary use name as runtime resource path

        [JsonIgnore]
        public IImmutableList<TransformEntity> Entities { get => _entities.ToImmutableList(); }
        ResourceInfo? IResource.Info { get; set; }

        public GameScene()
        {
            GameApplication.Exiting += Game_Exiting;
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

        public TransformEntity CreateEntity(string? name = null, TransformEntity? parent = null)
        {
            var e = ECSManager.CreateTransformEntity();

            if (!string.IsNullOrEmpty(name)) e.Name = name;
            e.Parent = parent;

            AddEntity(e);

            return e;
        }

        public void OnSerializing()
        {
            // sorted entities so that the parent get serialize first then its children
            _entitiesToSerialize.Clear();
            _entitiesToSerialize = TransformEntityHelper.SortByDepth(Entities);
        }

        public void OnDeserialized()
        {
            foreach (var entity in _entitiesToSerialize)
            {
                AddEntity(entity);
            }
        }

        bool _isActive = false;

        List<TransformEntity> _entities = [];
        [JsonInclude]
        List<TransformEntity> _entitiesToSerialize = [];

        private void Game_Exiting()
        {
            Dispose();
        }

        public void Dispose()
        {
            foreach (var e in _entities)
            {
                e.Dispose();
            }
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name) ? ((IResource)this).Info.ResourceName : Name;
        }
    }
}
