using CruZ.Components;
using CruZ.Utility;
using System;
using System.Collections.Generic;

namespace CruZ.Scene
{
    public static partial class SceneManager
    {
        //public static event Action<GameScene>? OnSceneLoaded;
        //public static event Action<GameScene>? OnCurrentSceneUnLoaded;

        public static void LoadScene(GameScene scene)
        {
            //if (_currentScene != null)
            //{
            //    UnloadCurrent();
            //}

            //_currentScene = scene;

            //foreach (var e in _currentScene.Entities)
            //{
            //    LoadEntity(e);
            //}

            //_currentScene.OnEntityAdded += LoadEntity;
            //_currentScene.OnEntityRemoved += UnloadEntity;

            //OnSceneLoaded?.Invoke(_currentScene);
            //Logging.PushMsg(string.Format("Scene {0} Loaded", _currentScene.Name));

            throw new NotImplementedException();
        }

        public static void UnloadCurrent()
        {
            //if (_currentScene == null)
            //{
            //    Logging.PushMsg("Scene no scene Unloaded");
            //    return;
            //}

            //var unloadScene = _currentScene;
            //_currentScene = null;

            //foreach (var e in unloadScene.Entities)
            //{
            //    UnloadEntity(e);
            //}

            //unloadScene.OnEntityAdded -= LoadEntity;
            //unloadScene.OnEntityRemoved -= UnloadEntity;

            //OnCurrentSceneUnLoaded?.Invoke(unloadScene);
            //Logging.PushMsg(string.Format("Scene {0} Unloaded", unloadScene.Name));
            throw new NotImplementedException();
        }

        //private static void LoadEntity(TransformEntity e)
        //{
        //    e.IsActive = true;
        //}

        //private static void UnloadEntity(TransformEntity e)
        //{
        //    e.IsActive = false;
        //}

        //static GameScene? _currentScene;
    }
}