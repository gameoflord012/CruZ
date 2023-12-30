using Assimp;
using CruZ.Components;
using CruZ.Serialization;
using CruZ.Utility;
using Newtonsoft.Json;
using System;
using System.IO;

namespace CruZ
{
    public static class SceneManager
    {
        public static GameScene? GetSceneFromFile(string scenePath)
        {
            using(var fileReader = new StreamReader(scenePath))
            {
                var json = fileReader.ReadToEnd();
                GameScene? scene = JsonConvert.DeserializeObject<GameScene>(json, _settings);

                if (scene == null) return null;

                scene.Name = Path.GetFileName(scenePath);
                return scene;
            }
        }

        public static void LoadScene(GameScene scene)
        {
            if (_currentScene != null)
            {
                UnloadCurrentScene();
            }

            _currentScene = scene;

            foreach (var e in scene.Entities)
            {
                LoadEntity(e);
            }

            scene.OnEntityAdded += LoadEntity;
            scene.OnEntityRemoved += UnloadEntity;

            OnSceneLoaded?.Invoke(_currentScene);
            Logging.PushMsg(string.Format("Scene {0} Loaded", scene.Name));
        }

        public static void UnloadCurrentScene()
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

        public static void SaveScene(GameScene? scene, string savePath)
        {
            if(scene == null)
            {
                Logging.PushMsg("No scene to save");
                return;
            }
            using var writer = new StreamWriter(savePath, false);
            var json = JsonConvert.SerializeObject(scene, _settings);
            writer.Write(json);
            writer.Flush();

            Logging.PushMsg("Scene {0} saved", scene.Name);
        }

        static SceneManager()
        {
            _settings = new();
            _settings.Converters.Add(new SerializableJsonConverter());
            _settings.Formatting = Newtonsoft.Json.Formatting.Indented;
        }

        static JsonSerializerSettings _settings;
        static GameScene? _currentScene;
         
        public static GameScene? CurrentScene { get => _currentScene; }

        public static event Action<GameScene>? OnSceneLoaded;
        public static event Action<GameScene>? OnCurrentSceneUnLoaded;
    }
}