using CruZ.Systems;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Draw = System.Drawing;

namespace CruZ.UI
{
    public partial class UIControl
    {
        static readonly int BOUND_THICKNESS = 2;
        static readonly XNA.Color DEFAULT_BACKGROUND_COLOR = XNA.Color.Red;

        #region Properties
        public UIControl? Parent { get => _parent; }
        public UIControl[] Childs => _childs.ToArray();

        public Draw.Point Location
        {
            get => new((int)_location.X, (int)_location.Y);
            set { _location.X = value.X; _location.Y = value.Y; }
        }
        public int Width { get => (int)_size.Width; set => _size.Width = value; }
        public int Height { get => (int)_size.Height; set => _size.Height = value; }

        public Color BackgroundColor = DEFAULT_BACKGROUND_COLOR;
        public bool Active = true;
        #endregion

        public void AddChild(UIControl child)
        {
            _childs.Add(child);

            child._parent = this;
            child.OnParentChanged(this);
        }

        public Draw.RectangleF GetRect()
        {
            return new Draw.RectangleF(_location.X, _location.Y, _size.Width, _size.Height);
        }

        public void RemoveChild(UIControl child)
        {
            if(!_childs.Remove(child))
            {
                throw new ArgumentException($"Fail to remove {child} ui control from {this}");
            }

            child._parent = null;
            child.OnParentChanged(null);
        }

        internal void InternalUpdate(UIInfo args)
        {
            if(!Active) return;

            _args = args;

            ProcessDragging(args);
            
            if (IsMouseHover())
            {
                if (args.InputInfo.MouseClick && !Dragging())
                {
                    OnMouseClick(args);
                }

                if (args.InputInfo.MouseStateChanges)
                {
                    OnMouseStateChange(args);
                }
            }

            OnUpdate(args);
        }

        internal void InternalDraw(UIInfo args)
        {
            if(!Active) return;

            _args = args;
            OnDraw(args);
        }

        protected bool IsMouseHover()
        {
            return GetRect().Contains(_args.MousePos().X, _args.MousePos().Y);
        }

        protected void ReleaseDrag()
        {
            s_GlobalDragObject = null;
        }

        protected virtual void OnMouseClick(UIInfo args)
        {
        }
        protected virtual void OnMouseStateChange(UIInfo args) { }
        protected virtual void OnParentChanged(UIControl? parent) { }
        protected virtual void OnUpdate(UIInfo args) { }
        protected virtual void OnDraw(UIInfo args)
        {
            args.SpriteBatch.DrawRectangle(_location, _size, BackgroundColor, BOUND_THICKNESS);
        }

        #region Dragging
        protected virtual object? OnStartDragging(UIInfo args) => null;
        protected virtual void OnUpdateDragging(UIInfo args) { }
        protected virtual bool OnReleaseDragging() => true;

        private void ProcessDragging(UIInfo args)
        {
            if (!Dragging() &&
                IsMouseHover() &&
                args.InputInfo.IsMouseHeldDown(MouseKey.Left) &&
                args.InputInfo.MouseMoving)
            {
                _dragObject = OnStartDragging(args);
                s_GlobalDragObject = _dragObject;
            }

            
            if(Dragging() && s_GlobalDragObject == _dragObject)
            {
                OnUpdateDragging(args);

                if(args.InputInfo.IsMouseHeldUp(MouseKey.Left))
                {
                    if(OnReleaseDragging())
                    {
                        _dragObject = null;
                        s_GlobalDragObject = null;
                    }
                }
            }
        }
        #endregion

        #region Private_Variables
        List<UIControl> _childs = [];
        UIControl? _parent;

        Vector2 _location = new(0, 0);
        Size2 _size = new(0, 0);

        object? _dragObject;

        UIInfo _args;
        #endregion
    }

    public partial class UIControl
    {
        private static object? s_GlobalDragObject = null;
        public static bool Dragging() { return s_GlobalDragObject != null; }
    }
}