using CruZ.Resource;
using CruZ.UI;
using CruZ.Utility;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace CruZ.Editor.UI
{
    internal class InfoTextWindow : UIControl
    {
        //string DisplayText = "Default";
        Dictionary<string, string> TextInfo = [];

        public InfoTextWindow()
        {
            Location = new(5, 3);

            _font = ResourceManager.LoadResource<SpriteFont>("default");
            _lineSpacing = _font.LineSpacing * _scale;
            _curRow = 0;
        }

        //public void SetText(string text)Regular
        //{
        //    _text = text;
        //}

        protected override void OnDraw(UIInfo info)
        {
            _sb = info.SpriteBatch;

            _curRow = 0;
            DrawString(Logging.GetMsg("Scene"));
            _curRow++;
            DrawString(Logging.GetMsg("Default"));
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

        //private string GetText(string key)
        //{
        //    if(TextInfo.ContainsKey(key)) return TextInfo[key];
        //    return "";
        //}

        SpriteFont _font;
        SpriteBatch? _sb;
        float _lineSpacing;
        float _curRow;
        float _scale = 0.8f;
        //string _text = "helloWorld";
    }
}
