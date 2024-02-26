using System;
using System.ComponentModel.Design;
using System.IO;

using CruZ.Resource;

namespace CruZ.Editor
{
    static class EditorGlobal
    {
        public static readonly XNA.Color UNIT_BOARD_COLOR = XNA.Color.DimGray;
        public static readonly XNA.Color BOARD_COLOR = XNA.Color.DarkGray;
        public static readonly int TARGET_FPS = 60;
        public static readonly float MAX_WORLD_DISTANCE = 10000;
        public static readonly float CENTER_CIRCLE_SIZE = 4;

        public static readonly string USER_PROJECT_PROFILE_DIR_NAME = ".cruzprofile";

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
                UserResource = new ResourceManager(UserResDir);
            }
        }

        public static string EditorResourceDir
        {
            get
            {
                if(string.IsNullOrEmpty(_editorResourceDir))
                    throw new InvalidOperationException("Set value first");

                return _editorResourceDir;
            }

            internal set
            {
                if(_editorResourceDir == value) return;
                _editorResourceDir = value;
                EditorResource = new(_editorResourceDir);
            }
        }

        public static string UserResDir { get => Path.Combine(UserProjectDir, "res"); }

        public static ResourceManager? EditorResource { get; private set; }
        public static ResourceManager? UserResource { get; private set; }

        static string? _userProjectDir;
        static string? _editorResourceDir;
    }
}
