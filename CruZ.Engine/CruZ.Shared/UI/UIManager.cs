using CruZ.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Timers;
using System;
using System.Collections.Generic;

namespace CruZ.UI
{
    public interface UIContext
    {
        event Action<GameTime>  DrawUI;
        event Action<GameTime>  UpdateUI;
        SpriteBatch             SpriteBatch { get; }
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

        private UIArgs GetArgs(GameTime gameTime)
        {
            UIArgs args = new();
            args.GameTime = gameTime;
            args.InputInfo = Input.Instance.GetInfo();
            args.SpriteBatch = _context.SpriteBatch;
            return args;
        }

        private void OnDrawUI(GameTime gameTime)
        {
            var args = GetArgs(gameTime);

            _spriteBatch.Begin();

            foreach (var control in Controls)
            {
                control.Draw(args);
            }

            _spriteBatch.End();
        }

        SpriteBatch         _spriteBatch => _context.SpriteBatch;
        UIContext           _context;
        List<UIControl>     _controls = [];
    }

    public partial class UIManager
    {
        public static void SetContext(UIContext context)
        {
            Instance = new(context);
        }

        public static UIManager? Instance { get; private set; }
        public static List<UIControl> Controls => Instance._controls;
    }
}