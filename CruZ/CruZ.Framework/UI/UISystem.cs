﻿using System;

using CruZ.Framework.GameSystem.ECS;
using CruZ.Framework.Input;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.Framework.UI
{
    internal class UISystem : EntitySystem
    {
        private UISystem()
        {
            _root = new();
        }

        public override void OnInitialize()
        {
            GameApplication.WindowResized += Window_Resized;
            ResizeRootBounds(GameApplication.GetGraphicsDevice().Viewport);
            _spriteBatch = new SpriteBatch(GameApplication.GetGraphicsDevice());
        }

        private void ResizeRootBounds(Viewport vp)
        {
            _root.Location = new(0, 0);
            _root.Width = vp.Width;
            _root.Height = vp.Height;
        }

        private void Window_Resized(Viewport viewport)
        {
            ResizeRootBounds(viewport);
        }

        protected override void OnUpdate(EntitySystemEventArgs args)
        {
            UIInfo info = GetInfo(args.GameTime);

            if (info.InputInfo.MouseClick && !UIControl.Dragging())
            {
                MouseClick?.Invoke(info);
            }

            foreach (var control in _root.GetTree())
            {
                control.InternalUpdate(info);
            }
        }

        protected override void OnDraw(EntitySystemEventArgs args)
        {
            var uiInfo = GetInfo(args.GameTime);

            _spriteBatch.Begin();

            foreach (var control in _root.GetTree())
            {
                control.InternalDraw(uiInfo);
            }

            _spriteBatch.End();
        }

        private UIInfo GetInfo(GameTime gameTime)
        {
            UIInfo info = new();

            info.SpriteBatch = _spriteBatch;
            info.GameTime = gameTime;
            info.InputInfo = InputManager.Info;

            return info;
        }

        readonly RootControl _root;
        SpriteBatch _spriteBatch;
        bool _isDisposed = false;

        protected override void OnDispose()
        {
            base.OnDispose();
            _isDisposed = true;
            _spriteBatch?.Dispose();
        }

        public static event Action<UIInfo>? MouseClick;

        internal static UISystem CreateContext()
        {
            if (_instance != null && !_instance._isDisposed)
                throw new InvalidOperationException("Require dispose");
            return _instance = new();
        }

        private static UISystem? _instance;
        public static RootControl Root => _instance._root;
    }
}
