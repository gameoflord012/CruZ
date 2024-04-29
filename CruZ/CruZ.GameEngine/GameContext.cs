using System;
using System.IO;
using System.Linq;
using System.Reflection;

using CruZ.GameEngine.Resource;
using CruZ.GameEngine.Utility;

namespace CruZ.GameEngine
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
                _internalResource = ResourceManager.From(Path.Combine(_gameResourceDir, ".internal"));
                InitializeInternalResource();
            }
        }

        public static ResourceManager GameResource
        {
            get => CheckNull(_gameResource);
        }

        public static ResourceManager InternalResource
        {
            get => CheckNull(_internalResource);
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
            _gameResource!.CopyResourceData(
                ResourceManager.From(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resource\\Internal\\")),
                ".internal");
        }

        static string? _gameResourceDir;

        static ResourceManager? _gameResource;
        static ResourceManager? _internalResource;
    }
}