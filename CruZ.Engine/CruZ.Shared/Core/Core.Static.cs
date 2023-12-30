using CruZ.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CruZ
{
    public partial class Core
    {
        private static Core? _instance;
        public static Core Instance { get => _instance ??= new Core(); }

        public static readonly string CONTENT_ROOT = "Content\\bin";

        public static event ActionDelegate? OnInitialize;
        public static event ActionDelegate? OnLoadContent;
        public static event ActionDelegate? OnEndRun;
        public static event OnExitingDelegate? OnExit;
        public static event CruZ_UpdateDelegate? OnUpdate;
        public static event CruZ_UpdateDelegate? OnDraw;

        public static void ChangeWindowSize(int width, int height)
        {
            Instance._graphics.IsFullScreen = false;
            Instance._graphics.PreferredBackBufferWidth = width;
            Instance._graphics.PreferredBackBufferHeight = height;
            Instance._graphics.ApplyChanges();
        }

        public static Viewport Viewport { get => Instance.GraphicsDevice.Viewport; }
    }
}
