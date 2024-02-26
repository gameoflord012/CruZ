using CruZ.Exception;
using CruZ.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CruZ.Scene
{
    public static partial class SceneManager
    {
        static SceneManager()
        {
            Type[] types = Assembly.GetEntryAssembly().GetTypes();

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

                    //try
                    //{
                        
                    //    //_sceneMethods[sceneName] = (GameScene)method.Invoke(null, BindingFlags.DoNotWrapExceptions, null, null, null);
                    //    //_sceneMethods[sceneName].ResourceInfo = ResourceInfo.Create(sceneName, true);
                    //}
                    //catch
                    //{
                    //    throw;
                    //}
                }
            }

        }

        public static GameScene GetRuntimeScene(string sceneName)
        {
            if (!_sceneMethods.ContainsKey(sceneName))
                throw new SceneAssetNotFoundException($"Asset {sceneName} not available");

            var method = _sceneMethods[sceneName];
            GameScene scene;

            try
            {
                scene = (GameScene)method.Invoke(null, BindingFlags.DoNotWrapExceptions, null, null, null);
                scene.ResourceInfo = ResourceInfo.Create(null, sceneName);
            }
            catch(System.Exception e)
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