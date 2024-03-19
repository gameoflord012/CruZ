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

        public Camera(GameWindow window)
        {
            ViewPortWidth = window.ClientBounds.Width;
            ViewPortHeight = window.ClientBounds.Height;
            _virtualWidth = ViewPortWidth;
            _virtualHeight = ViewPortHeight;
            _window = window;

            window.ClientSizeChanged += Window_ClientSizeChanged;
            UpdateViewportSize();
        }

        public Vector2 PointToCoordinate(Point p)
        {
            var ndc = new Vector2(
                p.X / ViewPortWidth  - 0.5f, 
                -p.Y / ViewPortHeight - 0.5f);

            var inv = Matrix.Invert(ViewProjectionMatrix());
            var world = Vector4.Transform(ndc, inv);

            return new Vector2(world.X, world.Y);
        }

        public Point CoordinateToPoint(Vector2 coord)
        {
            var world = new Vector4(coord.X, coord.Y, 0, 1);
            var ndc = Vector4.Transform(world, ViewProjectionMatrix());

            // Convert ndc to screen
            var screen = new Vector2(
                (ndc.X + 1) / 2f * ViewPortWidth, 
                (-ndc.Y + 1) / 2f * ViewPortHeight);

            return new Point(screen.X.RoundToInt(), screen.Y.RoundToInt());
        }

        public Vector2 ScreenToWorldRatio()
        { 
            return new(
                ViewPortWidth * Zoom / VirtualHeight,
                ViewPortHeight * Zoom / ViewPortHeight
            );

        }

        /// <summary>
        /// Convert from world coordinate to camera coodinate, which camera's viewport TL is -size / 2
        /// </summary>
        /// <returns></returns>
        
        public Matrix ViewProjectionMatrix()
        {
            return ViewMatrix() * ProjectionMatrix();
        }

        public Matrix ViewMatrix()
        {
            var mat = Matrix.Identity;
            mat *= Matrix.CreateTranslation(-CameraOffset.X, -CameraOffset.Y, 0);
            mat *= Matrix.CreateScale(Zoom);
            return mat;
        }

        public Matrix ProjectionMatrix()
        {
            return Matrix.CreateOrthographicOffCenter(
                -VirtualWidth / 2, VirtualWidth / 2,
                VirtualHeight / 2, -VirtualHeight / 2, 
                -GameConstants.MAX_WORLD_DISTANCE, GameConstants.MAX_WORLD_DISTANCE);
        }

        public float VirtualWidth
        {
            get => _virtualWidth;
            set { _virtualWidth = value; }
        }

        public float VirtualHeight
        {
            get => _virtualHeight;
            set => _virtualHeight = value;
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

        private void Window_ClientSizeChanged(object? sender, EventArgs e)
        {
            UpdateViewportSize();
        }

        private void UpdateViewportSize()
        {
            ViewPortWidth = _window.ClientBounds.Width;
            ViewPortHeight = _window.ClientBounds.Height;
        }

        public Vector2 CameraOffset;
        public float Zoom = 1;

        float _viewPortWidth;
        float _viewPortHeight;
        float _virtualWidth = 19;
        float _virtualHeight = 10;

        GameWindow _window;
    }
}