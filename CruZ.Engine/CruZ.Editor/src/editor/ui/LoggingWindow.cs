using CruZ.Resource;
using CruZ.Service;
using CruZ.UI;

using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

namespace CruZ.Editor.UI
{
    internal class LoggingWindow : UIControl
    {
        Dictionary<string, string> TextInfo = [];

        public LoggingWindow()
        {
            Location = new(5, 3);

            _resource = EditorContext.EditorResource;
            _font = _resource.Load<SpriteFont>("default");
            _lineSpacing = _font.LineSpacing * _scale;
            _curRow = 0;
        }

        protected override void OnDraw(UIInfo info)
        {
            _sb = info.SpriteBatch;
            _curRow = 0;
            DrawString(LogService.GetMsg("Fps"));
            _curRow++;
            DrawString(LogService.GetMsg("Scene"));
            _curRow++;
            DrawString(LogService.GetMsg("Default"));
        }

        private void DrawString(string s)
        {
            _sb?.DrawString(
                _font, s,
                new XNA.Vector2(
                    Location.X, Location.Y + _curRow * _lineSpacing), 
                XNA.Color.Black
                , 0, new XNA.Vector2(0, 0), _scale, SpriteEffects.None, 0
                );
        }

        SpriteFont _font;
        SpriteBatch? _sb;
        float _lineSpacing;
        float _curRow;
        float _scale = 0.7f;

        ResourceManager _resource;
    }
}
