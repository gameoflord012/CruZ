using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using CruZ.Common;
using CruZ.Common.Resource;

namespace CruZ.Editor.Global
{
    static class EditorContext
    {
        public static string UserProjectDir
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
                GameContext.GameResourceDir = Path.Combine(_userProjectDir, "res\\");
            }
        }

        public static string UserProjectBinDir { get; internal set; }
        
        public static string UserResourceDir { get => Path.Combine(UserProjectDir, "Resource\\"); }

        public static string UserProfileDir { get => Path.Combine(UserProjectDir, EditorConstants.USER_PROFILE_DIR_NAME); }

        //public static Assembly PlatformAssembly => Assembly.LoadFile(Path.Combine(UserProjectBinDir, "CruZ.DesktopGL.dll"));

        public static Assembly UserProjectAssembly
        {
            get
            {
                _userProjectAssembly ??= Assembly.LoadFile(Path.Combine(UserProjectBinDir, "Game.AnimalGang.DesktopGL.dll"));
                return _userProjectAssembly;
            }
        }

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
        static Assembly? _userProjectAssembly;
    }
}
