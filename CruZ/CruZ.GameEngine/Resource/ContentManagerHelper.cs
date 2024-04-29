using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Content;

namespace CruZ.GameEngine.Resource
{
    internal static class ContentManagerHelper
    {
        public static T LoadFromRoot<T>(this ContentManager contentManager, string resourceName, string contentRoot, Func<string, Type, string>? resolver = null)
        {
            if(Path.IsPathRooted(resourceName)) throw new ArgumentException(resourceName);
            contentManager.RootDirectory = contentRoot;
            contentManager.AssetNameResolver = resolver;
            T loaded = contentManager.Load<T>(resourceName);
            contentManager.RootDirectory = default;
            contentManager.AssetNameResolver = default;
            return loaded;
        }
    }
}
