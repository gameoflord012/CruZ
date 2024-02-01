using CruZ.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Timers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;

namespace CruZ.UI
{
    public interface UIContext
    {
        event Action<GameTime>  DrawUI;
        event Action<GameTime>  UpdateUI;
    }

    public class UIArgs
    {
        public GameTime     GameTime;
        public InputInfo    InputInfo;
        public SpriteBatch  SpriteBatch;
    }

    public partial class UIManager
    {
        UIManager(UIContext context)
        {
            _context = context;

            _context.DrawUI += OnDrawUI;
            _context.UpdateUI += OnUpdateUI;
        }

        private void OnUpdateUI(GameTime gameTime)
        {
            UIArgs args = GetArgs(gameTime);

            foreach (var control in Controls)
            {
                control.Update(args);
            }
        }

        private void OnDrawUI(GameTime gameTime)
        {
            var args = GetArgs(gameTime);

            args.SpriteBatch.Begin();

            foreach (var control in Controls)
            {
                control.Draw(args);
            }

            args.SpriteBatch.End();
        }

        private UIArgs GetArgs(GameTime gameTime)
        {
            if(_spriteBatch == null)
                _spriteBatch = GameApplication.GetSpriteBatch();

            UIArgs args = new();
            args.GameTime = gameTime;
            args.InputInfo = Input.Info;
            args.SpriteBatch = _spriteBatch;
            return args;
        }

        SpriteBatch?        _spriteBatch = null;
        UIContext           _context;
        List<UIControl>     _controls = [];
    }

    public partial class UIManager
    {
        public static void CreateContext(UIContext context)
        {
            _instance = new(context);
        }

        private static UIManager? _instance;
        public static List<UIControl> Controls => _instance._controls;
    }
}