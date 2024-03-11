using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using CruZ.Common;
using CruZ.Common.Scene;
using CruZ.Editor.Global;

namespace CruZ.Editor.Scene
{
    public static class SceneManager
    {
        static SceneManager()
        {
            Assembly userAssembly = EditorContext.UserProjectAssembly;

            Type[] types;

            try
            {
                types = userAssembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {

                throw e;
            }
            
            var sceneClasses = types
                .Where(type => Attribute.IsDefined(type, typeof(SceneAssetClassAttribute)));

            foreach (var clazz in sceneClasses)
            {
                foreach (var method in clazz
                    .GetMethods()
                    .Where(mt => Attribute.IsDefined(mt, typeof(SceneAssetMethodAttribute))))
                {
                    var classAttribute = clazz.GetCustomAttribute(typeof(SceneAssetClassAttribute)) as SceneAssetClassAttribute;
                    var methodAttribute = method.GetCustomAttribute(typeof(SceneAssetMethodAttribute)) as SceneAssetMethodAttribute;

                    var sceneName = classAttribute.AssetClassId + "\\" +

                                    (string.IsNullOrEmpty(methodAttribute.AssetMethodId) ?
                                        "" : methodAttribute.AssetMethodId + "\\") +

                                    method.Name;

                    _sceneMethods[sceneName] = method;
                }
            }
        }

        public static GameScene GetRuntimeScene(string sceneName)
        {
            if (!_sceneMethods.ContainsKey(sceneName))
                throw new SceneAssetNotFoundException($"Runtime Scene {sceneName} not available");

            var method = _sceneMethods[sceneName];
            GameScene scene;

            try
            {
                scene = (GameScene)method.Invoke(null, null);
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
            return _sceneMethods.Keys.ToArray();
        }

        private static Dictionary<string, MethodInfo> _sceneMethods = [];
    }
}