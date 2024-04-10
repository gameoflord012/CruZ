using CruZ.Framework;
using CruZ.Framework.Input;
using CruZ.Framework.UI;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

namespace CruZ.Framework.UI
{
    interface IUIController
    {
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        void Update(GameTime gameTime);
        void Initialize();
    }

    public struct UIInfo
    {
        public GameTime GameTime;
        public IInputInfo InputInfo;
        public SpriteBatch? SpriteBatch;

        public Point MousePos()
        {
            return new(
                InputInfo.CurMouse.Position.X,
                InputInfo.CurMouse.Position.Y);
        }
    }

    public partial class UIManager : IUIController
    {
        private UIManager()
        {
            _root = new();
        }

        void IUIController.Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var uiInfo = GetInfo(gameTime, spriteBatch);

            uiInfo.SpriteBatch.Begin();

            foreach (var control in GetTree(_root))
            {
                control.InternalDraw(uiInfo);
            }

            uiInfo.SpriteBatch.End();
        }

        void IUIController.Update(GameTime gameTime)
        {
            UIInfo info = GetInfo(gameTime, null);

            if (info.InputInfo.MouseClick && !UIControl.Dragging())
            {
                MouseClick?.Invoke(info);
            }

            foreach (var control in GetTree(_root))
            {
                control.InternalUpdate(info);
            }
        }

        void IUIController.Initialize()
        {
            GameApplication.WindowResized += Window_Resized;
            ResizeRootBounds(GameApplication.GetGraphicsDevice().Viewport);
        }

        private void Window_Resized(Viewport viewport)
        {
            ResizeRootBounds(viewport);
        }

        private void ResizeRootBounds(Viewport vp)
        {
            _root.Location = new(0, 0);
            _root.Width = vp.Width;
            _root.Height = vp.Height;
        }

        private UIInfo GetInfo(GameTime gameTime, SpriteBatch? sp)
        {
            UIInfo info = new();

            info.SpriteBatch = sp;
            info.GameTime = gameTime;
            info.InputInfo = InputManager.Info;

            return info;
        }

        readonly RootControl _root;
    }

    public partial class UIManager
    {
        public static event Action<UIInfo>? MouseClick;

        internal static IUIController CreateContext()
        {
            return _instance = new();
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
                if (node.GetRect().Contains(mouseX, mouseY))
                    contains.Add(node);
            }

            return contains.ToArray();
        }

        private static UIControl[] GetTree(UIControl control)
        {
            List<UIControl> list = [];
            list.Add(control);

            for (int i = 0; i < list.Count; i++)
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