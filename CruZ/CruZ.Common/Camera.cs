using System;

using CruZ.Common.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.Common
{
    public enum ProjectionOffset
    {
        Center, // When projection top left at -width / 2, -height / 2
        Topleft // When projection top left at 0, 0
    }

    public partial class Camera
    {
        //public event Action OnCameraValueChanged;

        public Camera(Viewport viewport)
        {
            ViewPortWidth = viewport.Width;
            ViewPortHeight = viewport.Height;
            _virtualWidth = ViewPortWidth;
            _virtualHeight = ViewPortHeight;
        }

        public Camera(int vpWidth, int vpHeight) : this(new(0, 0, vpWidth, vpHeight))
        {

        }

        public Vector2 PointToCoordinate(Point p)
        {
            var normalize_x = (p.X / ViewPortWidth - 0.5f);
            var normalize_y = (p.Y / ViewPortHeight - 0.5f);

            var coord = new Vector2(normalize_x * VirtualWidth, normalize_y * VirtualHeight);
            coord += Position;

            return coord;
        }

        public Point CoordinateToPoint(Vector2 coord)
        {
            coord -= _position;

            var normalize_x = 0.5f + coord.X / VirtualWidth;
            var normalize_y = 0.5f + coord.Y / VirtualHeight;

            return new(
                FunMath.RoundInt(normalize_x * ViewPortWidth),
                FunMath.RoundInt(normalize_y * ViewPortHeight));
        }

        public Matrix ViewMatrix()
        {
            float scaleX = ViewPortWidth / VirtualWidth;
            float scaleY = ViewPortHeight / VirtualHeight;

            var mat = Matrix.Identity;
            mat *= Matrix.CreateScale(scaleX, scaleY, 1);
            mat *= Matrix.CreateTranslation(-Position.X * scaleX, -Position.Y * scaleY, 0);
            return mat;
        }

        public Matrix ProjectionMatrix()
        {
            return
                Matrix.CreateOrthographicOffCenter(
                    -VirtualWidth / 2f, VirtualWidth / 2f,
                    VirtualHeight / 2f, -VirtualHeight / 2f, 0, 1);
        }

        public Vector2 ScreenToWorldScale()
        {
            return new(
                VirtualWidth / ViewPortWidth,
                VirtualHeight / ViewPortHeight);
        }

        public Vector2 WorldToScreenScale()
        {
            return new(
                ViewPortWidth / VirtualWidth,
                ViewPortHeight / VirtualHeight);
        }

        public float VirtualWidth
        {
            get => _virtualWidth / Zoom.X;
            set { _virtualWidth = value; }
        }

        public float VirtualHeight
        {
            get => (PreserveRatio ? VirtualWidth / Ratio : _virtualHeight / Zoom.Y);
            set { _virtualHeight = value; }
        }

        public float ViewPortWidth
        {
            get => _viewPortWidth;
            set { _viewPortWidth = value; }
        }

        public float ViewPortHeight
        {
            get => _viewPortHeight;
            set { _viewPortHeight = value; }
        }

        public Vector2 Position
        {
            get => _position;
            set { _position = value; }
        }

        private Vector2 _position = Vector2.Zero;

        public bool PreserveRatio = true;
        public float Ratio => ViewPortWidth / ViewPortHeight;

        public Vector2 Zoom
        {
            get => _zoom;
            set
            {
                value.X = MathF.Max(0.0001f, value.X);
                value.Y = MathF.Max(0.0001f, value.Y);
                _zoom = value;
            }
        }

        private Vector2 _zoom = new(1, 1);
        private float _viewPortWidth;
        private float _viewPortHeight;
        private float _virtualWidth = 19;
        private float _virtualHeight = 10;
    }
}