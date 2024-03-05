using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CruZ.Common.Scene
{
    public static partial class SceneManager
    {
        static SceneManager()
        {
            Assembly platformAssembly = Assembly.LoadFrom("D:\\monogame-projects\\CruZ_GameEngine\\SandBox\\AnimalGang\\AnimalGang.DesktopGL\\bin\\Debug\\net8.0\\CruZ.DesktopGL.dll");
            Assembly clientAssembly = Assembly.LoadFrom("D:\\monogame-projects\\CruZ_GameEngine\\SandBox\\AnimalGang\\AnimalGang.DesktopGL\\bin\\Debug\\net8.0\\Game.AnimalGang.DesktopGL.dll");
            Type sceneAssetClassAttributeType = platformAssembly.GetType("CruZ.Common.Scene." + nameof(SceneAssetClassAttribute), true);
            Type sceneAssetMethodAttributeType = platformAssembly.GetType("CruZ.Common.Scene." + nameof(SceneAssetMethodAttribute), true);

            Type[] types = clientAssembly.GetTypes();
            var sceneClasses = types
                .Where(type => Attribute.IsDefined(type, sceneAssetClassAttributeType));

            foreach (var clazz in sceneClasses)
            {
                foreach (var method in clazz
                    .GetMethods()
                    .Where(mt => Attribute.IsDefined(mt, sceneAssetMethodAttributeType)))
                {
                    var classAttribute = clazz.GetCustomAttribute(sceneAssetClassAttributeType) as SceneAssetClassAttribute;
                    var methodAttribute = method.GetCustomAttribute(sceneAssetMethodAttributeType) as SceneAssetMethodAttribute;

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