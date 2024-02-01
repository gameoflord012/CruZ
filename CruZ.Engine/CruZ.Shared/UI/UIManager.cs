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
            _spriteBatch ??= new SpriteBatch(ApplicationContext.GraphicsDevice);

            UIArgs args = new();
            args.GameTime = gameTime;
            args.InputInfo = Input.Instance.GetInputInfo();
            args.SpriteBatch = _spriteBatch;
            return args;
        }

        SpriteBatch?        _spriteBatch;
        UIContext           _context;
        List<UIControl>     _controls = [];
    }

    public partial class UIManager
    {
        public static void CreateContext(UIContext context)
        {
            Instance = new(context);
        }

        public static UIManager? Instance { get; private set; }
        public static List<UIControl> Controls => Instance._controls;
    }
}