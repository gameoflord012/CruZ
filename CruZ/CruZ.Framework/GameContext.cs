using System;
using System.IO;

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

        private static T CheckNull<T>(T? value)
        {
            return value ?? throw new InvalidOperationException("Set value first");
        }


        private static void InitializeInternalResource()
        {
            PathHelper.UpdateFolderContents(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resource\\Internal\\"),
                Path.Combine(_gameResourceDir, ".internal\\"),
                "*", true, true);
        }

        static string? _gameResourceDir;
        static ResourceManager? _gameResource;
    }
}