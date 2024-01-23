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

            var assetClasses = types
                .Where(type => Attribute.IsDefined(type, typeof(SceneAssetClassAttribute)));

            foreach (var clazz in assetClasses)
            {
                foreach (var method in clazz
                    .GetMethods()
                    .Where(mt => Attribute.IsDefined(mt, typeof(SceneAssetMethodAttribute))))
                {
                    var classAttribute = clazz.GetCustomAttribute(typeof(SceneAssetClassAttribute)) as SceneAssetClassAttribute;
                    var methodAttribute = method.GetCustomAttribute(typeof(SceneAssetMethodAttribute)) as SceneAssetMethodAttribute;

                    var assetPath = classAttribute.AssetClassId + "\\" +
                                    methodAttribute.AssetMethodId + "\\" +
                                    method.Name;

                    try
                    {
                        SceneAssets[assetPath] = (GameScene)method.Invoke(null, BindingFlags.DoNotWrapExceptions, null, null, null);
                        SceneAssets[assetPath].ResourcePath = assetPath;
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

        }

        public static GameScene GetSceneAssets(string assetName)
        {
            if (!SceneAssets.ContainsKey(assetName)) 
                throw new KeyNotFoundException($"Asset {assetName} not available");

            return SceneAssets[assetName];
        }

        public static Dictionary<string, GameScene> SceneAssets = [];
    }
}