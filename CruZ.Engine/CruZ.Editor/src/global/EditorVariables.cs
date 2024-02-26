

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CruZ.Editor
{
    static class EditorVariables
    {
        public static readonly XNA.Color UNIT_BOARD_COLOR = XNA.Color.DimGray;
        public static readonly XNA.Color BOARD_COLOR = XNA.Color.DarkGray;
        public static readonly int TARGET_FPS = 60;
        public static readonly float MAX_WORLD_DISTANCE = 10000;
        public static readonly float CENTER_CIRCLE_SIZE = 4;

        public static readonly string USER_PROJECT_PROFILE_DIR_NAME = ".cruzprofile";
        public static string UserResDir { get; internal set; }
        public static string UserProjectProfileDir { get; internal set; }
    }
}
