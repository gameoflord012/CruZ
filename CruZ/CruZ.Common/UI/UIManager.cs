using CruZ.Common.Input;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

namespace CruZ.Common.UI
{
    public interface UIContext
    {
        event Action<GameTime>  DrawUI;
        event Action<GameTime>  UpdateUI;
        event Action            InitializeUI;
    }

    public struct UIInfo
    {
        public GameTime     GameTime;
        public IInputInfo   InputInfo;
        public SpriteBatch  SpriteBatch;

        public DRAW.Point MousePos()
        {
            return new(
                InputInfo.CurMouse.Position.X, 
                InputInfo.CurMouse.Position.Y);
        }
    }

    public partial class UIManager
    {
        UIManager(UIContext context)
        {
            _context = context;

            _context.DrawUI += Context_DrawUI;
            _context.UpdateUI += Context_UpdateUI;
            _context.InitializeUI += Context_Initialize;
        }

        private void Context_Initialize()
        {
            GameApplication.RegisterWindowResize(GameApp_WindowResize);
            GameApp_WindowResize(GameApplication.Viewport);
        }

        private void GameApp_WindowResize(Viewport viewport)
        {
            Root.Location = new(0, 0);
            Root.Width = viewport.Width;
            Root.Height = viewport.Height;
        }

        private void Context_UpdateUI(GameTime gameTime)
        {
            UIInfo info = GetInfo(gameTime);

            if(info.InputInfo.MouseClick && !UIControl.Dragging())
            {
                MouseClick?.Invoke(info);
            }

            foreach (var control in GetTree(_root))
            {
                control.InternalUpdate(info);
            }
        }

        private void Context_DrawUI(GameTime gameTime)
        {
            var args = GetInfo(gameTime);

            args.SpriteBatch.Begin();

            foreach (var control in GetTree(_root))
            {
                control.InternalDraw(args);
            }

            args.SpriteBatch.End();
        }

        private UIInfo GetInfo(GameTime gameTime)
        {
            if(_spriteBatch == null)
                _spriteBatch = GameApplication.GetSpriteBatch();

            UIInfo info = new();
            
            info.GameTime = gameTime;
            info.InputInfo = InputManager.Info;
            info.SpriteBatch = _spriteBatch;

            return info;
        }

        SpriteBatch?    _spriteBatch = null;
        UIContext       _context;
        RootControl     _root = new();
    }

    public partial class UIManager
    {
        public static event Action<UIInfo>? MouseClick;

        public static void CreateContext(UIContext context)
        {
            _instance = new(context);
        }

        public static UIControl[] GetContains(int mouseX, int mouseY)
        {
            return GetContains(_instance._root, mouseX, mouseY);
        }

        public static UIControl[] GetContains(UIControl root, int mouseX, int mouseY)
        {
            List<UIControl> contains = [];

            foreach (var node in GetTree(root))
            {
                if(node.GetRect().Contains(mouseX, mouseY))
                    contains.Add(node);
            }

            return contains.ToArray();
        }

        //public static object? s_GlobalDragObject { get => _instance._dragObject; set => _instance._dragObject = value; }

        //public static bool Dragging() { return s_GlobalDragObject != null; }

        private static UIControl[] GetTree(UIControl control)
        {
            List<UIControl> list = [];
            list.Add(control);

            for(int i = 0; i < list.Count; i++)
            {
                foreach (var child in list[i].Childs)
                {
                    list.Add(child);
                }
            }

            return list.ToArray();
        }

        private static UIManager? _instance;
        public static RootControl Root => _instance._root;
    }
}