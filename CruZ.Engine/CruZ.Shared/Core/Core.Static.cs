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

        public static event Action? OnInitialize;
        public static event Action? OnLoadContent;
        public static event Action? OnEndRun;
        public static event Action<Object, EventArgs>? OnExit;
        public static event Action<GameTime>? OnUpdate;
        public static event Action<GameTime>? OnDraw;
        public static event Action<GameTime>? OnLateDraw;

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
