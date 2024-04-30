using System;
using System.Collections.Generic;
using System.Linq;

using CruZ.GameEngine;
using CruZ.GameEngine.Input;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.GameEngine.GameSystem.UI
{
    internal class UISystem : EntitySystem
    {
        private UISystem()
        {
            _root = new();
            _root.AddBranch("Main");
        }

        public override void OnInitialize()
        {
            GameApplication.WindowResized += Window_Resized;
            ResizeRootBounds(GameApplication.GetGraphicsDevice().Viewport);
            _gd = GameApplication.GetGraphicsDevice();
            _spriteBatch = new SpriteBatch(_gd);
        }

        private void ResizeRootBounds(Viewport vp)
        {
            _root.Control.Location = new(0, 0);
            _root.Control.Width = vp.Width;
            _root.Control.Height = vp.Height;
        }

        private void Window_Resized(Viewport viewport)
        {
            ResizeRootBounds(viewport);
        }

        protected override void OnUpdate(EntitySystemEventArgs args)
        {
            UpdateUIComponents(args.ActiveEntities.GetAllComponents<UIComponent>());
            var info = CreateUIInfo(args.GameTime);

            if (info.InputInfo.MouseClick && !UIControl.Dragging())
            {
                MouseClick?.Invoke(info);
            }

            foreach (var control in _root.Control.GetTree())
            {
                control.InternalUpdate(info);
            }
        }

        protected override void OnDraw(EntitySystemEventArgs args)
        {
            UpdateUIComponents(args.ActiveEntities.GetAllComponents<UIComponent>());
            var uiInfo = CreateUIInfo(args.GameTime);

            _gd.SetRenderTarget(RenderTargetSystem.UIRT);
            _gd.Clear(Color.Transparent);

            _spriteBatch.Begin();
            foreach (var control in _root.Control.GetTree())
            {
                control.InternalDraw(uiInfo);
            }
            _spriteBatch.End();

            _gd.SetRenderTarget(null);
        }

        private void UpdateUIComponents(List<UIComponent> newComponents)
        {
            // unroot previous UIComponents
            foreach (var component in _rootedUIComponents)
            {
                _root.Control.RemoveChild(component.EntryControl);
            }

            _rootedUIComponents.Clear();

            // root new UIComponents
            foreach (var uiComponent in newComponents)
            {
                _root.Control.AddChild(uiComponent.EntryControl);
                _rootedUIComponents.Add(uiComponent);
            }
        }

        // components that attached to main branch
        List<UIComponent> _rootedUIComponents = [];

        private UIInfo CreateUIInfo(GameTime gameTime)
        {
            UIInfo info = new();

            info.SpriteBatch = _spriteBatch;
            info.GameTime = gameTime;
            info.InputInfo = InputManager.Info;

            return info;
        }

        GraphicsDevice _gd;

        UIRoot _root;
        SpriteBatch _spriteBatch = null!;
        bool _isDisposed = false;

        public override void Dispose()
        {
            base.Dispose();
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
        public static UIRoot Root => _instance._root;
    }
}
