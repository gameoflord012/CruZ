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
            var inv = Matrix.Invert(ViewMatrix());
            var screen = new Vector4(p.X - VirtualWidth / 2f, p.Y - VirtualHeight / 2f, 0, 1);
            var world = Vector4.Transform(screen, inv);

            return new Vector2(world.X / world.W,  world.Y / world.W);
        }

        public Point CoordinateToPoint(Vector2 coord)
        {
            var world = new Vector4(coord.X, coord.Y, 0, 1);
            var screen = Vector4.Transform(world, ViewMatrix());

            return new Point(
                (screen.X / screen.W + VirtualWidth / 2f).RoundToInt(), 
                (screen.Y / screen.W + VirtualHeight / 2f).RoundToInt());
        }

        /// <summary>
        /// Convert from world coordinate to camera coodinate, which camera's viewport TL is -size / 2
        /// </summary>
        /// <returns></returns>
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