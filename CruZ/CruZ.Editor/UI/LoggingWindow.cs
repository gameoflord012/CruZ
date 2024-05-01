using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CruZ.GameEngine.GameSystem.UI;
using CruZ.GameEngine.Resource;
using MonoGame.Extended.BitmapFonts;
using CruZ.GameEngine;
using CruZ.GameEngine.Utility;

namespace CruZ.Editor.UI
{
    internal class LoggingWindow : UIControl
    {
        Dictionary<string, string> TextInfo = [];

        public LoggingWindow()
        {
            Location = new(5, 3);

            _fontScale = 0.7f;
            _resource = GameApplication.GameResource;
            _font = _resource.Load<BitmapFont>(".internal\\Fonts\\Fixedsys.fnt");
            _lineSpacing = _font.LineHeight * _fontScale;
        }

        protected override void OnDraw(UIInfo info)
        {
            _sb = info.SpriteBatch;
            _curRow = -1;

            DrawLogMsg("Fps");
            DrawLogMsg("Scene");
            DrawLogMsg("CursorCoord");
            DrawLogMsg("CameraWorldCoord");
        }

        private void DrawLogMsg(string key)
        {
            if(!string.IsNullOrEmpty(LogManager.GetMsg(key)))
            {
                _curRow++;
                DrawString(LogManager.GetMsgFormmated(key));
            }
        }

        private void DrawString(string s)
        {
            _sb?.DrawString(
                _font, s,
                new Vector2(Location.X, Location.Y + _curRow * _lineSpacing),
                Color.Black, 
                0, new Vector2(0, 0), _fontScale, SpriteEffects.None, 0
                );
        }

        BitmapFont _font;
        SpriteBatch? _sb;
        float _lineSpacing;
        float _curRow;
        float _fontScale;

        ResourceManager _resource;
    }
}
