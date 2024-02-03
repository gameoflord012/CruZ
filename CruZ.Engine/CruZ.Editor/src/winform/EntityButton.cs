//using CruZ.Components;
//using CruZ.Editor.Systems;
//using CruZ.Systems;
//using System;
//using System.ComponentModel.DataAnnotations;
//using System.Diagnostics;
//using System.Drawing;
//using System.Windows.Forms;

//namespace CruZ.Editor.Controls
//{
//    public class EntityButton : Button, ICanUndo
//    {
//        public TransformEntity AttachedEntity { get => _attachedEntity; }

//        public EntityButton(TransformEntity attachedEntity)
//        {
//            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
//            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

//            this._attachedEntity = attachedEntity;
//            _attachedEntity.OnActiveStateChanged += AttachedEntity_OnActiveStateChanged;
//            _attachedEntity.RemoveFromWorldEvent += AttachedEntity_OnRemoveFromWorld;

//            Size = new(ButtonSize, ButtonSize);
//        }

//        private void AttachedEntity_OnRemoveFromWorld(object? sender, EventArgs e)
//        {
//            Parent?.Controls.Remove(this);
//            Dispose();
//        }

//        private void AttachedEntity_OnActiveStateChanged(object? sender, bool e)
//        {
//            Visible = e;
//        }

//        protected override void OnParentChanged(EventArgs e)
//        {
//            base.OnParentChanged(e);

//            if(_parent != null) _parent.Invalidated -= Parent_Invalidated;
//            _parent = Parent;
//            if(_parent != null) _parent.Invalidated += Parent_Invalidated;
//        }

//        private void Parent_Invalidated(object? sender, InvalidateEventArgs e)
//        {
//            UpdateButtonPosition();
//        }

//        private void UpdateButtonPosition()
//        {
//            var ePoint = Camera.Main.CoordinateToPoint(_attachedEntity.Transform.Position);
//            SetCenter(ePoint);
//        }

//        protected override void OnMouseMove(MouseEventArgs mevent)
//        {
//            base.OnMouseMove(mevent);

//            if (_isDragging)
//            {
//                var mouseScreen = PointToScreen(mevent.Location);
//                var mouseClient = Parent.PointToClient(mouseScreen);

//                SetCenter(mouseClient);
//                UpdateEntityPosition();
//            }
//        }

//        private void UpdateEntityPosition()
//        {
//            _attachedEntity.Transform.Position = Camera.Main.PointToCoordinate(GetCenter());
//        }

//        protected override void OnMouseDown(MouseEventArgs mevent)
//        {
//            base.OnMouseDown(mevent);

//            if (mevent.Button == MouseButtons.Left && _isMouseEntered)
//            {
//                _isDragging = true;
//            }
//        }

//        protected override void OnMouseUp(MouseEventArgs mevent)
//        {
//            base.OnMouseUp(mevent);

//            if (mevent.Button == MouseButtons.Left)
//            {
//                _isDragging = false;
//                UndoService.Capture(this);
//            }
//        }

//        private void SetCenter(Point p)
//        {
//            Location = new(
//                p.X - (Size.Width + 1) / 2,
//                p.Y - (Size.Height + 1) / 2);

//            Invalidate();
//        }

//        private Point GetCenter()
//        {
//            return new(
//                Location.X + (Size.Width + 1) / 2,
//                Location.Y + (Size.Height + 1) / 2);
//        }

//        protected override void OnMouseEnter(EventArgs e)
//        {
//            base.OnMouseHover(e);

//            _isMouseEntered = true;
//        }

//        protected override void OnMouseLeave(EventArgs e)
//        {
//            base.OnMouseLeave(e);

//            _isMouseEntered = false;
//        }

//        public object CaptureState()
//        {
//            return Location;
//        }

//        public void RestoreState(object state)
//        {
//            Location = (Point)state;
//            UpdateEntityPosition();
//            Invalidate();
//        }

//        public bool StatesIdentical(object stateA, object stateB)
//        {
//            var locA = (Point)stateA;
//            var locB = (Point)stateB;

//            return locA.Equals(locB);
//        }

//        private TransformEntity _attachedEntity;

//        private bool _isMouseEntered = false;
//        private bool _isDragging = false;

//        private readonly int ButtonSize = 10;
//        Control? _parent;
//    }
//}
