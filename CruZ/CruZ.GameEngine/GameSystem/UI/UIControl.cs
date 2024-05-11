using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;

using CruZ.GameEngine.Input;
using CruZ.GameEngine.Utility;

using Microsoft.Xna.Framework;

using RectangleF = System.Drawing.RectangleF;

namespace CruZ.GameEngine.GameSystem.UI
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class UIControl
    {
        private const int BorderThickness = 1;
        private static readonly Color DefaultBackGroundColor = Color.Red;

        public UIControl()
        {
            _childs = [];
            IsActive = true;
            BackgroundColor = DefaultBackGroundColor;
        }

        public void AddChild(UIControl child)
        {
            _childs.Add(child);

            child._parent = this;
            child.OnParentChanged(this);
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

        /// <summary>
        /// Get all children controls and itself under mouse point
        /// </summary>
        public UIControl[] GetRaycastControls(int pointX, int pointY)
        {
            List<UIControl> contains = [];

            foreach(var node in GetTree())
            {
                if(node.Rect.Contains(pointX, pointY))
                    contains.Add(node);
            }

            return contains.ToArray();
        }

        public IEnumerable<UIControl> GetTree()
        {
            List<UIControl> list = [];
            list.Add(this);

            for(int i = 0; i < list.Count; i++)
            {
                foreach(var child in list[i].Childs)
                {
                    list.Add(child);
                }
            }

            return list.Where(e => e.IsActive);
        }

        internal void InternalUpdate(UpdateUIEventArgs args)
        {
            _inputInfo = args.InputInfo;

            // dragging
            UpdateDragging(args);

            // update mouse events
            if(IsMouseHovered())
            {
                if(args.InputInfo.MouseClick && !Dragging())
                {
                    OnMouseClick(_inputInfo);
                }

                if(args.InputInfo.MouseStateChanges)
                {
                    OnMouseStateChange(_inputInfo);
                }
            }

            // call update
            OnUpdate(args);
        }

        internal void InternalDraw(DrawUIEventArgs args)
        {
            OnDraw(args);
        }

        protected bool IsMouseHovered()
        {
            return Rect.Contains(_inputInfo.MousePos().X, _inputInfo.MousePos().Y);
        }

        protected virtual void OnMouseClick(IInputInfo inputInfo)
        { }

        protected virtual void OnMouseStateChange(IInputInfo inputInfo)
        { }

        protected virtual void OnParentChanged(UIControl? parent)
        { }

        protected virtual void OnUpdate(UpdateUIEventArgs args)
        { }

        protected virtual void OnDraw(DrawUIEventArgs args)
        {
            args.SpriteBatch.DrawRectangle(Rect, BackgroundColor, BorderThickness);
        }

        #region Dragging
        protected virtual object? OnStartDragging(IInputInfo inputInfo)
            => null;

        protected virtual void OnUpdateDragging(UpdateUIEventArgs args)
        { }

        protected virtual bool OnReleaseDragging()
            => true;

        private void UpdateDragging(UpdateUIEventArgs args)
        {
            if(!Dragging() &&
                IsMouseHovered() &&
                _inputInfo.IsMouseHeldDown(MouseKey.Left) &&
                _inputInfo.MouseMoving)
            {
                _dragObject = OnStartDragging(_inputInfo);
                s_dragObject = _dragObject;
            }


            if(Dragging() && s_dragObject == _dragObject)
            {
                OnUpdateDragging(args);

                if(_inputInfo.IsMouseHeldUp(MouseKey.Left))
                {
                    if(OnReleaseDragging())
                    {
                        _dragObject = null;
                        s_dragObject = null;
                    }
                }
            }
        }

        #endregion

        public bool IsActive
        {
            get;
            set;
        }

        public UIControl? Parent
        { get => _parent; }

        public UIControl[] Childs
            => _childs.ToArray();

        public Color BackgroundColor;

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

        private List<UIControl> _childs;
        private UIControl? _parent;
        private object? _dragObject;
        private IInputInfo _inputInfo;

        public void Dispose()
        {
            Parent?.RemoveChild(this);
        }

        private static object? s_dragObject;
        public static bool Dragging()
        { return s_dragObject != null; }
    }
}
