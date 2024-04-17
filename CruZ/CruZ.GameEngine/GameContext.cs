using System;
using System.IO;
using System.Linq;
using System.Reflection;

using CruZ.Framework.Resource;
using CruZ.Framework.Utility;

namespace CruZ.Framework
{
    public static class GameContext
    {
        public static string GameResourceDir
        {
            get => CheckNull(_gameResourceDir);
            set
            {
                if (_gameResourceDir == value) return;
                _gameResourceDir = value;
                _gameResource = ResourceManager.From(_gameResourceDir);
                InitializeInternalResource();
            }
        }

        public static ResourceManager GameResource
        {
            get => CheckNull(_gameResource);
            private set => _gameResource = value;
        }

        public static Func<AssemblyName, Assembly?> AssemblyResolver 
        { 
            get => (resolvingAss) => AppDomain.CurrentDomain.GetAssemblies()
                    .First(domainAss => domainAss.FullName == resolvingAss.FullName);
        }

        private static T CheckNull<T>(T? value)
        {
            return value ?? throw new InvalidOperationException("Set value first");
        }

        private static void InitializeInternalResource()
        {
            PathHelper.CopyFolder(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resource\\Internal\\"),
                Path.Combine(GameResourceDir, ".internal\\"),
                "*", true, true);
        }

        static string? _gameResourceDir;
        static ResourceManager? _gameResource;
    }
}