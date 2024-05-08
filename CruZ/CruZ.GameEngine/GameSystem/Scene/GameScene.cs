using CruZ.GameEngine.Resource;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace CruZ.GameEngine.GameSystem.Scene
{
    public class GameScene : IDisposable
    {
        public event Action<TransformEntity>? EntityAdded;
        public event Action<TransformEntity>? EntityRemoved;

        public string Name 
        {
            get => _name;
            set
            {
                if(_name == value) return;
                SceneRoot.Name = value;
                _name = value;
            }
        }

        string _name = "Noname Scene";

        [JsonIgnore]
        public IImmutableList<TransformEntity> Entities { get => _entities.ToImmutableList(); }
        //ResourceInfo? IResource.Info { get; set; }

        public GameScene()
        {
            GameApplication.Exiting += Game_Exiting;
            SceneRoot = ECSManager.CreateEntity();
            SceneRoot.IsActive = false;
        }

        private void AddEntity(TransformEntity e)
        {
            if (_entities.Contains(e)) return;
            _entities.Add(e);

            e.IsActive = true;
            e.RemovedFromWorld += Entity_RemovedFromWorld;

            EntityAdded?.Invoke(e);
        }

        private void Entity_RemovedFromWorld(TransformEntity e)
        {
            if (!_entities.Contains(e))
                throw new ArgumentException($"Entity \"{e}\" not in scene {this}");

            _entities.Remove(e);

            e.IsActive = false;
            e.RemovedFromWorld -= Entity_RemovedFromWorld;
            e.Dispose();

            EntityRemoved?.Invoke(e);
        }

        public TransformEntity CreateEntity(string? name = null, TransformEntity? parent = null)
        {
            var e = ECSManager.CreateEntity();

            if (!string.IsNullOrEmpty(name)) e.Name = name;
            e.Parent = parent ?? SceneRoot;

            AddEntity(e);

            return e;
        }

        TransformEntity SceneRoot;

        List<TransformEntity> _entities = [];

        private void Game_Exiting()
        {
            Dispose();
        }

        public void Dispose()
        {
            foreach (var e in _entities.ToImmutableList())
            {
                e.Dispose();
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
