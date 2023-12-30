using CruZ.Utility;
using Microsoft.Xna.Framework.Graphics;
using System.Numerics;

namespace CruZ
{
    public class Camera
    {
        public Camera(Viewport viewport)
        {
            _viewport = viewport;
        }

        private static Camera _mainCamera;

        public static Camera GetMain()
        {
            if (_mainCamera == null)
            {
                _mainCamera = new(CruZ.Viewport);
            }
            return _mainCamera;
        }

        public static Camera Main
        {
            get => GetMain();
            set => _mainCamera = value;
        }

        public Vector3 PointToCoordinate(Vector3 p)
        {
            var normalize_x = (p.X / _viewport.Width - 0.5f);
            var normalize_y = (p.Y / _viewport.Height - 0.5f);

            var coord = new Vector3(normalize_x * VirtualWidth, normalize_y * VirtualHeight, 0);
            coord -= Position;

            return coord;
        }

        public Vector2 CoordinateToPoint(Vector3 coord)
        {
            coord += Position;

            var normalize_x = 0.5f + coord.X / VirtualWidth;
            var normalize_y = 0.5f + coord.Y / VirtualHeight;

            return new(
                FunMath.RoundInt(normalize_x * _viewport.Width),
                FunMath.RoundInt(normalize_y * _viewport.Height));
        }

        public Matrix4x4 ViewMatrix()
        {
            return
                Matrix4x4.CreateTranslation(Position) *

                Matrix4x4.CreateTranslation(
                    VirtualWidth / 2f,
                    VirtualHeight / 2f,
                    0f) *

                Matrix4x4.CreateScale(
                    _viewport.Width / VirtualWidth,
                    _viewport.Height / VirtualHeight, 1);
        }

        public Vector2 ScreenToSpaceScale()
        {
            return new(
                VirtualWidth / _viewport.Width,
                VirtualHeight / _viewport.Height);
        }

        public float VirtualWidth = 1980;
        public float VirtualHeight = 1080;

        public Vector3 Position = Vector3.Zero;

        Viewport _viewport;
    }
}