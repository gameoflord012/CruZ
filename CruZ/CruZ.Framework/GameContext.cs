using System;

using CruZ.Framework.Resource;

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

        static string? _gameResourceDir;
        static ResourceManager? _gameResource;
    }
}