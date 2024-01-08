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

                    SceneAssets[assetPath] = (GameScene)method.Invoke(null, null);
                }
            }

        }

        public static Dictionary<string, GameScene> SceneAssets = [];
    }
}