using CruZ.Components;
using CruZ.Systems;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace CruZ.Editor.Controls
{
    public class EntityButton : Button
    {
        public EntityButton(TransformEntity attachedEntity)
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            this._attachedEntity = attachedEntity;

            Size = new(ButtonSize, ButtonSize);
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            Parent.Invalidated += (sender, args) => UpdateButtonPosition();
        }

        private void UpdateButtonPosition()
        {
            var ePoint = Camera.Main.CoordinateToPoint(_attachedEntity.Transform.Position);
            SetCenter(ePoint);
        }

        protected override void OnMouseMove(MouseEventArgs mevent)
        {
            base.OnMouseMove(mevent);

            if (_isDragging)
            {
                var mouseScreen = PointToScreen(mevent.Location);
                var mouseClient = Parent.PointToClient(mouseScreen);

                SetCenter(mouseClient);
                _attachedEntity.Transform.Position = Camera.Main.PointToCoordinate(mouseClient);
            }
        }
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);

            if(mevent.Button == MouseButtons.Left && _isMouseEntered)
            {
                _isDragging = true;
            }
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);

            if(mevent.Button == MouseButtons.Left)
            {
                _isDragging = false;
            }
        }

        private void SetCenter(Point p)
        {
            Location = new(
                p.X - (Size.Width + 1) / 2,
                p.Y - (Size.Height + 1) / 2);

            Invalidate();
        }

        private Point GetCenter()
        {
            return new(
                Location.X + (Size.Width + 1) / 2,
                Location.Y + (Size.Height + 1) / 2);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseHover(e);

            _isMouseEntered = true;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            _isMouseEntered = false;
        }

        private TransformEntity _attachedEntity;

        private bool _isMouseEntered = false;
        private bool _isDragging = false;

        private readonly int ButtonSize = 10;
    }
}
