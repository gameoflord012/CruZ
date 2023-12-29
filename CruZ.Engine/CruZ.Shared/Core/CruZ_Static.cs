using CruZ_Engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CruZ_Engine
{
    public partial class CruZ
    {
        private static CruZ _instance;
        public static CruZ Instance()
        {
            if (_instance == null)
                _instance = new CruZ();
            return _instance;
        }

        public static readonly string CONTENT_ROOT = "Content";
        public static Viewport Viewport { get => Instance().GraphicsDevice.Viewport; }
    
        public static void ChangeWindowSize(int width, int height)
        {
            Instance()._graphics.IsFullScreen = false;
            Instance()._graphics.PreferredBackBufferWidth = width;
            Instance()._graphics.PreferredBackBufferHeight = height;
            Instance()._graphics.ApplyChanges();
        }
    }
}
