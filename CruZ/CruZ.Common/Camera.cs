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

            return new Vector2(world.X / world.W, world.Y / world.W);
        }

        public Point CoordinateToPoint(Vector2 coord)
        {
            var world = new Vector4(coord.X, coord.Y, 0, 1);
            var screen = Vector4.Transform(world, ViewMatrix());

            return new Point(
                (screen.X + VirtualWidth / 2f).RoundToInt(),
                (screen.Y + VirtualHeight / 2f).RoundToInt());
        }

        /// <summary>
        /// Convert from world coordinate to camera coodinate, which camera's viewport TL is -size / 2
        /// </summary>
        /// <returns></returns>
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

        public Vector2 ScreenToWorldScale()
        {
            return new(
                VirtualWidth / Zoom / ViewPortWidth,
                VirtualHeight / Zoom / ViewPortHeight);
        }

        public Vector2 WorldToScreenScale()
        {
            return new(
                ViewPortWidth / (VirtualWidth / Zoom),
                ViewPortHeight / (VirtualHeight / Zoom));
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

        public Vector2 CameraOffset;

        public float Zoom = 1;

        //public Vector2 Position
        //{
        //    get => _position;
        //    set { _position = value; }
        //}

        //public Vector2 Center
        //{
        //    get => new(Position.X + VirtualWidth / 2f, Position.Y + VirtualHeight / 2f);
        //    set => _position = new(value.X - VirtualWidth / 2f, value.Y - VirtualHeight / 2f);
        //}

        private Vector2 _position = Vector2.Zero;
        private float _ratio => ViewPortWidth / ViewPortHeight;

        private float _viewPortWidth;
        private float _viewPortHeight;
        private float _virtualWidth = 19;
        private float _virtualHeight = 10;
    }
}