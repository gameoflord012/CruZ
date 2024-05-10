using System;
using System.Collections.Generic;

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
            _root.Control.Rect = new(0, 0, vp.Width, vp.Height);
        }

        private void Window_Resized(Viewport viewport)
        {
            ResizeRootBounds(viewport);
        }

        protected override void OnUpdate(SystemEventArgs args)
        {
            UpdateUIComponents(args.ActiveEntities.GetAllComponents<UIComponent>());
            var info = new UpdateUIEventArgs(args.GameTime, GameInput.GetLastInfo());

            foreach(var control in _root.Control.GetTree())
            {
                control.InternalUpdate(info);
            }
        }

        protected override void OnDraw(SystemEventArgs args)
        {
            UpdateUIComponents(args.ActiveEntities.GetAllComponents<UIComponent>());
            var drawUIArgs = new DrawUIEventArgs(args.GameTime, _spriteBatch);

            _gd.SetRenderTarget(RenderTargetSystem.UIRT);
            _gd.Clear(Color.Transparent);

            _spriteBatch.Begin();
            foreach(var control in _root.Control.GetTree())
            {
                control.InternalDraw(drawUIArgs);
            }
            _spriteBatch.End();

            _gd.SetRenderTarget(null);
        }

        private void UpdateUIComponents(List<UIComponent> newComponents)
        {
            // unroot previous UIComponents
            foreach(var component in _rootedUIComponents)
            {
                _root.Control.RemoveChild(component.EntryControl);
            }

            _rootedUIComponents.Clear();

            // root new UIComponents
            foreach(var uiComponent in newComponents)
            {
                _root.Control.AddChild(uiComponent.EntryControl);
                _rootedUIComponents.Add(uiComponent);
            }
        }

        // components that attached to main branch
        private List<UIComponent> _rootedUIComponents = [];
        private GraphicsDevice _gd;
        private UIRoot _root;
        private SpriteBatch _spriteBatch;
        private bool _isDisposed;

        public override void Dispose()
        {
            base.Dispose();
            _isDisposed = true;
            _spriteBatch?.Dispose();
        }

        internal static UISystem CreateContext()
        {
            if(_instance != null && !_instance._isDisposed)
                throw new InvalidOperationException("Require dispose");
            return _instance = new();
        }
        internal static UISystem Instance
        { get => _instance ?? throw new NullReferenceException(); }

        private static UISystem? _instance;

        public static UIRoot Root
            => Instance._root;

    }
}
