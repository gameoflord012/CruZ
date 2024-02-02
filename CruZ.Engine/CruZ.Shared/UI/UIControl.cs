using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using Draw = System.Drawing;

namespace CruZ.UI
{
    public class UIControl
    {
        public UIControl[] Childs => _childs.ToArray();

        public Draw.Point Location
        {
            get => new((int)_location.X, (int)_location.Y);
            set { _location.X = value.X; _location.Y = value.Y; }
        }

        public int Width { get => (int)_size.Width; set => _size.Width = value; }
        public int Height { get => (int)_size.Height; set => _size.Height = value; }
        public UIControl? Parent { get => _parent; }

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
            _childs.Remove(this);

            child._parent = this;
            child.OnParentChanged(null);
        }

        internal void InternalUpdate(UIArgs args)
        {
            _args = args;

            if(args.InputInfo.IsAnyMouseDown() && IsMouseHover())
            {
                OnMouseDown(args);
            }

            OnUpdate(args);
        }

        internal void InternalDraw(UIArgs args)
        {
            _args = args;
            OnDraw(args);
        }

        protected virtual void OnMouseDown(UIArgs args) { }

        protected virtual void OnParentChanged(UIControl? parent) { }

        protected virtual void OnUpdate(UIArgs args) { }

        protected virtual void OnDraw(UIArgs args)
        {
            //if (IsMouseHover())
            //    args.SpriteBatch.FillRectangle(_location, _size, 
            //        new Color(50, 0, 0, 5), 1f);

            args.SpriteBatch.DrawRectangle(_location, _size, Color.Red);

        }

        protected bool IsMouseHover()
        {
            return GetRect().Contains(_args.MousePos().X, _args.MousePos().Y);
        }
        
        List<UIControl> _childs = [];

        UIControl? _parent;
        Vector2 _location = new(0, 0);
        Size2 _size = new(0, 0);
        UIArgs _args;
    }
}