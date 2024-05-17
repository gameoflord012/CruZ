using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace CruZ.GameEngine.GameSystem.Scene
{
    public class GameScene
    {
        public event Action<TransformEntity>? EntityAdded;
        public event Action<TransformEntity>? EntityRemoved;

        public GameScene()
        {
            if(Environment.CurrentManagedThreadId != GameApplication.ThreadId)
            {
                throw new InvalidOperationException("Different thread");
            }

            _sceneRoot = ECSManager.CreateEntity();
            _sceneRoot.IsActive = false;
            Name = "New Scene";
            _entities = [];
        }

        public void AddEntity(TransformEntity e)
        {
            if(_entities.Contains(e)) return;
            _entities.Add(e);

            e.IsActive = true;
            e.Destroying += Entity_Destroying;

            EntityAdded?.Invoke(e);
        }

        public TransformEntity CreateEntity(string? name = null, TransformEntity? parent = null)
        {
            var e = ECSManager.CreateEntity();

            if(!string.IsNullOrEmpty(name)) e.Name = name;
            e.Parent = parent ?? _sceneRoot;

            AddEntity(e);

            return e;
        }

        public void Destroy()
        {
            foreach(var entity in _entities.ToImmutableArray())
            {
                entity.Destroy();
            }
        }

        private void Entity_Destroying(TransformEntity e)
        {
            RemoveEntity(e);
        }

        private void RemoveEntity(TransformEntity e)
        {
            if(!_entities.Contains(e))
            {
                throw new ArgumentException($"Entity \"{e}\" not in scene {this}");
            }

            _entities.Remove(e);

            e.IsActive = false;
            e.Destroying -= Entity_Destroying;

            EntityRemoved?.Invoke(e);
        }

        public string Name
        {
            get => _name;
            set
            {
                if(_name == value) return;
                _sceneRoot.Name = value;
                _name = value;
            }
        }

        public TransformEntity SceneRoot
        {
            get => _sceneRoot;
        }

        private TransformEntity _sceneRoot;
        private List<TransformEntity> _entities;
        private string _name;

        public override string ToString()
        {
            return Name;
        }
    }
}
