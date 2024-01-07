using CruZ.Components;
using CruZ.Utility;
using System;
using System.Collections.Generic;

namespace CruZ
{
    public static class SceneManager
    {
        public static event Action<GameScene>? OnSceneLoaded;
        public static event Action<GameScene>? OnCurrentSceneUnLoaded;

        public static GameScene? CurrentScene { get => _currentScene; }

        //public static GameScene GetSceneResource(string uri)
        //{
        //    var scene = ResourceManager.LoadResource<GameScene>(uri);
        //    if (scene == null)
        //    {
        //        scene = _avaiableScene[uri];
        //        ResourceManager.CreateResource(uri, scene);
        //    }

        //    return scene;
        //}

        public static void SetActive(GameScene scene)
        {
            if (_currentScene != null)
            {
                UnactivateCurrent();
            }

            _currentScene = scene;

            foreach (var e in _currentScene.Entities)
            {
                LoadEntity(e);
            }

            _currentScene.OnEntityAdded += LoadEntity;
            _currentScene.OnEntityRemoved += UnloadEntity;

            OnSceneLoaded?.Invoke(_currentScene);
            Logging.PushMsg(string.Format("Scene {0} Loaded", _currentScene.Name));
        }

        public static void UnactivateCurrent()
        {
            if (_currentScene == null)
            {
                Logging.PushMsg("Scene no scene Unloaded");
                return;
            }

            var unloadScene = _currentScene;
            _currentScene = null;

            foreach (var e in unloadScene.Entities)
            {
                UnloadEntity(e);
            }

            unloadScene.OnEntityAdded -= LoadEntity;
            unloadScene.OnEntityRemoved -= UnloadEntity;

            OnCurrentSceneUnLoaded?.Invoke(unloadScene);
            Logging.PushMsg(string.Format("Scene {0} Unloaded", unloadScene.Name));
        }

        private static void LoadEntity(TransformEntity e)
        {
            e.IsActive = true;
        }

        private static void UnloadEntity(TransformEntity e)
        {
            e.RemoveFromWorld();
        }

        static GameScene? _currentScene;
    }
}