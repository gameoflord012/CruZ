using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem.Scene;

namespace CruZ.Editor.Scene
{
    public static class SceneManager
    {
        static void FindUserAssemblyScene()
        {
            Assembly userAssembly = EditorContext.GameAssembly;
            Type[] types = userAssembly.GetTypes();

            var sceneClasses = types
                .Where(type => Attribute.IsDefined(type, typeof(SceneFactoryClassAttribute)));

            foreach (var clazz in sceneClasses)
            {
                foreach (var method in clazz
                    .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                    .Where(mt => Attribute.IsDefined(mt, typeof(SceneFactoryMethodAttribute))))
                {
                    var classAttribute = (SceneFactoryClassAttribute)clazz.GetCustomAttribute(typeof(SceneFactoryClassAttribute))!;
                    var methodAttribute = (SceneFactoryMethodAttribute)method.GetCustomAttribute(typeof(SceneFactoryMethodAttribute))!;
                    var sceneName = Path.Combine(classAttribute.Id, methodAttribute.Id, method.Name);

                    _sceneMethods[sceneName] = method;
                }
            }
        }

        public static GameScene GetRuntimeScene(string sceneName)
        {
            FindUserAssemblyScene();

            if (!_sceneMethods.ContainsKey(sceneName))
                throw new SceneAssetNotFoundException($"Runtime Scene {sceneName} not available");

            var method = _sceneMethods[sceneName];
            GameScene scene;

            try
            {
                object? instance = method.IsStatic ? null : Activator.CreateInstance(method.DeclaringType!)!;
                scene = (GameScene)method.Invoke(instance, null)!;
                scene.Name = sceneName;
            }
            catch (Exception e)
            {
                throw new RuntimeSceneLoadException($"Problem with loading {sceneName}", e);
            }

            return scene;
        }

        public static string[] GetSceneNames()
        {
            FindUserAssemblyScene();

            return _sceneMethods.Keys.ToArray();
        }

        private static Dictionary<string, MethodInfo> _sceneMethods = [];
    }
}