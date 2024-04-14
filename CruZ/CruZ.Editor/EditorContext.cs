using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using CruZ.Framework;
using CruZ.Framework.Resource;

namespace CruZ.Editor
{
    static class EditorContext
    {
        public static string GameProjectDir
        {
            get
            {
                if (string.IsNullOrEmpty(_userProjectDir))
                    throw new InvalidOperationException("Set value first");

                return _userProjectDir;
            }

            internal set
            {
                if (_userProjectDir == value) return;
                _userProjectDir = value;
                GameContext.GameResourceDir = UserResourceDir;
            }
        }

        public static string UserResourceDir { get => Path.Combine(GameProjectDir, "Resource\\"); }

        public static string UserProfileDir { get => Path.Combine(GameProjectDir, EditorConstants.USER_PROFILE_DIR_NAME); }

        public static Assembly GameAssembly { get => _gameAssembly ?? throw new InvalidOperationException("Invalid Context"); set => _gameAssembly = value; }

        public static string EditorResourceDir
        {
            get
            {
                if (string.IsNullOrEmpty(_editorResourceDir))
                    throw new InvalidOperationException("Set value first");

                return _editorResourceDir;
            }

            internal set
            {
                if (_editorResourceDir == value) return;
                _editorResourceDir = value;
                EditorResource = ResourceManager.From(_editorResourceDir);
            }
        }

        public static ResourceManager UserResource
        {
            get => GameContext.GameResource;
        }

        public static ResourceManager EditorResource
        {
            get => _editorResource ?? throw new InvalidOperationException("Set value first");
            private set => _editorResource = value;
        }

        static string? _userProjectDir;
        static string? _editorResourceDir;
        static ResourceManager? _editorResource;
        static Assembly? _gameAssembly;
    }
}
