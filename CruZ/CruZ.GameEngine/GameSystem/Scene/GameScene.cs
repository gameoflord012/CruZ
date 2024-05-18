using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;

namespace CruZ.GameEngine.GameSystem.Scene
{
    public partial class GameScene
    {
        public event Action<TransformEntity>? EntityAdded;
        public event Action<TransformEntity>? EntityRemoved;

        internal GameScene(string sceneName = "New Scene")
        {
            GameApplication.CheckThread();

            _sceneRoot = ECSManager.CreateEntity();
            _sceneRoot.IsActive = false;
            _entities = [];

            Name = sceneName;

            OnInitialize();
        }

        public TransformEntity CreateEntity(string? name = null, TransformEntity? parent = null)
        {
            var e = ECSManager.CreateEntity();

            if(!string.IsNullOrEmpty(name))
            {
                e.Name = name;
            }

            e.Parent = parent ?? _sceneRoot;

            AddEntity(e);

            return e;
        }

        public void AddEntity(TransformEntity e)
        {
            if(_entities.Contains(e)) return;
            _entities.Add(e);

            e.IsActive = true;
            e.Destroying += Entity_Destroying;

            EntityAdded?.Invoke(e);
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

        public void Destroy()
        {
            s_allocatedScenes.Remove(this);

            foreach(var entity in _entities.ToImmutableArray())
            {
                entity.Destroy();
            }
        }

        public override string ToString()
        {
            return Name;
        }

        protected virtual void OnInitialize()
        {

        }

        private void Entity_Destroying(TransformEntity e)
        {
            RemoveEntity(e);
        }

        public string Name
        {
            get => _sceneRoot.Name;
            set => _sceneRoot.Name = value;
        }

        public TransformEntity RootEntity
        {
            get => _sceneRoot;
        }

        private TransformEntity _sceneRoot;
        private List<TransformEntity> _entities;
    }

    public partial class GameScene
    {
        static GameScene()
        {
            s_allocatedScenes = [];
        }

        public static GameScene Create(string? name = null)
        {
            GameScene scene = new();
            scene.Name = string.IsNullOrEmpty(name) ? scene.GetType().Name : name;
            s_allocatedScenes.Add(scene);
            return scene;
        }

        public static int AllocatedCount
        {
            get => s_allocatedScenes.Count;
        }

        private static List<GameScene> s_allocatedScenes;
    }

}
