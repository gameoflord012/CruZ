using CruZ.GameEngine.Input;
using CruZ.GameEngine.Utility;

using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;

using RectangleF = System.Drawing.RectangleF;

namespace CruZ.GameEngine.GameSystem.UI
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class UIControl
    {
        public UIControl()
        {
             _childs = [];
        }

        static readonly int BorderThickness = 3;

        static readonly Color DefaultBackGroundColor = Color.Red;

        public UIControl? Parent { get => _parent; }

        public UIControl[] Childs => _childs.ToArray();

        public Color BackgroundColor = DefaultBackGroundColor;
        
        public bool IsActive = true;

        public void AddChild(UIControl child)
        {
            _childs.Add(child);

            child._parent = this;
            child.OnParentChanged(this);
        }

        public void RemoveChild(UIControl child)
        {
            if (!_childs.Remove(child))
            {
                throw new ArgumentException($"Fail to remove {child} ui control from {this}");
            }

            child._parent = null;
            child.OnParentChanged(null);
        }

        /// <summary>
        /// Get all children controls and itself under mouse point
        /// </summary>
        public UIControl[] GetRaycastControls(int pointX, int pointY)
        {
            List<UIControl> contains = [];

            foreach (var node in GetTree())
            {
                if (node.Rect.Contains(pointX, pointY))
                    contains.Add(node);
            }

            return contains.ToArray();
        }

        public IImmutableList<UIControl> GetTree(bool getActiveNodeOnly = true)
        {
            List<UIControl> list = [];
            list.Add(this);

            for (int i = 0; i < list.Count; i++)
            {
                foreach (var child in list[i].Childs)
                {
                    list.Add(child);
                }
            }

            return list.Where(e => e.IsActive).ToImmutableList();
        }

        internal void InternalUpdate(UIInfo args)
        {
            _args = args;

            // dragging
            ProcessDragging(args);

            // update mouse events
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

            // call update
            OnUpdate(args);
        }

        internal void InternalDraw(UIInfo args)
        {
            _args = args;
            OnDraw(args);
        }

        protected bool IsMouseHover()
        {
            return Rect.Contains(_args.MousePos().X, _args.MousePos().Y);
        }

        protected void ReleaseDrag()
        {
            _globalDragObject = null;
        }

        protected virtual void OnMouseClick(UIInfo args) { }

        protected virtual void OnMouseStateChange(UIInfo args) { }

        protected virtual void OnParentChanged(UIControl? parent) { }

        protected virtual void OnUpdate(UIInfo args) { }

        protected virtual void OnDraw(UIInfo args)
        {
            args.SpriteBatch.DrawRectangle(Rect, BackgroundColor, BorderThickness);
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
                _globalDragObject = _dragObject;
            }


            if (Dragging() && _globalDragObject == _dragObject)
            {
                OnUpdateDragging(args);

                if (args.InputInfo.IsMouseHeldUp(MouseKey.Left))
                {
                    if (OnReleaseDragging())
                    {
                        _dragObject = null;
                        _globalDragObject = null;
                    }
                }
            }
        }

        public void Dispose()
        {
            if (Parent != null)
            {
                Parent.RemoveChild(this);
            }
        }
        #endregion

        #region Private_Variables
        public RectangleF Rect;

        public float Width
        {
            get => Rect.Width;
            set => Rect.Width = value;
        }

        public float Height
        {
            get => Rect.Height;
            set => Rect.Height = value;
        }

        public Vector2 Location
        {
            get => new(Rect.X, Rect.Y);
            set
            {
                Rect.X = value.X;
                Rect.Y = value.Y;
            }
        }
        
        List<UIControl> _childs;
        UIControl? _parent;

        object? _dragObject;
        UIInfo _args;
        #endregion

        private static object? _globalDragObject = null;
        public static bool Dragging() { return _globalDragObject != null; }
    }
}