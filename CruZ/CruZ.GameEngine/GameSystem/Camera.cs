using System;

using CruZ.GameEngine.Utility;

using Microsoft.Xna.Framework;

namespace CruZ.GameEngine.GameSystem
{
    public class Camera
    {
        private static Camera? _mainCamera;
        public static Camera Main
        {
            get => _mainCamera ?? throw new InvalidOperationException("Main camera is not assigned");
            set => _mainCamera = value;
        }

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
            /*
            first get ndc from point, then ndc can be convert
            back to world coordinate by apply inverse view projection matrix
            which use to get ndc in the first place
             */

            var ndc = new Vector2(
                p.X / ViewPortWidth * 2f - 1f,
                -p.Y / ViewPortHeight * 2f - 1f);

            var inv = Matrix.Invert(ViewProjectionMatrix());
            var world = Vector4.Transform(ndc, inv);

            return new Vector2(world.X, world.Y);
        }

        public Point CoordinateToPoint(Vector2 coord)
        {
            /*
            first get screen ndc from world coordinate
            then from ndc convert to screen coord
             */
            var world = new Vector4(coord.X, coord.Y, 0, 1);
            var ndc = Vector4.Transform(world, ViewProjectionMatrix());

            // Convert ndc to screen
            var screen = new Vector2(
                (ndc.X + 1) / 2f * ViewPortWidth,
                (-ndc.Y + 1) / 2f * ViewPortHeight);

            return new Point(screen.X.RoundToInt(), screen.Y.RoundToInt());
        }

        /// <summary>
        /// A ratio use to convert from world magnitude to screen magnitude if multiply with it
        /// </summary>
        public Vector2 ScreenToWorldRatio()
        {
            return new(
                ViewPortWidth * Zoom / VirtualWidth,
                -ViewPortHeight * Zoom / VirtualHeight // because descartes is -y of screen coordinate
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

        /// <summary>
        /// Centered world coordinate to camera offset then apply scale, 
        /// so it will zoom in the sreen center
        /// </summary>
        public Matrix ViewMatrix()
        {
            var mat = Matrix.Identity;
            mat *= Matrix.CreateTranslation(-CameraOffset.X, -CameraOffset.Y, 0);
            mat *= Matrix.CreateScale(Zoom);
            return mat;
        }

        /// <summary>
        /// Create orthographic projection with given rect <br/>
        /// Rect's TL = (-VirtualWidth / 2, -VirtualHeight / 2) <br/>
        /// Rect's BR is opposite
        /// </summary>
        public Matrix ProjectionMatrix()
        {
            return Matrix.CreateOrthographicOffCenter(
                -VirtualWidth / 2, VirtualWidth / 2,
                -VirtualHeight / 2, VirtualHeight / 2,
                -GameConstants.MaxWorldDistance.Z, GameConstants.MaxWorldDistance.Z);
        }

        public float ViewPortWidth
        {
            get => _viewPortWidth;
            set => _viewPortWidth = value;
        }

        public float ViewPortHeight
        {
            get => _viewPortHeight;
            set => _viewPortHeight = value;
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

        /// <summary>
        /// CameraOffset's camera center in world coordinate
        /// </summary>
        public Vector2 CameraOffset
        {
            get => _cameraOffset;
            set
            {
                _cameraOffset = value;
                LogManager.SetMsg($"<{_cameraOffset.X} {_cameraOffset.Y}>", "CameraWorldCoord");
            }
        }

        public Vector2 _cameraOffset;
        public float Zoom = 1;

        /// <summary>
        /// If true, the virtual height will be calculated relative to virtual width and the screen ratio <br/>
        /// By doing so, the camera will not stretch render objects and preserves it ratio while allowing to resize the window <br/>
        /// Will ommit the set value on virtual height
        /// </summary>
        public bool PreserveScreenRatio = false;

        private float ScreenRatio => _viewPortWidth / _viewPortHeight;

        /// <summary>
        /// Projection width
        /// </summary>
        public float VirtualWidth
        {
            get => _virtualWidth;
            set { _virtualWidth = value; }
        }

        /// <summary>
        /// Projection height
        /// </summary>
        public float VirtualHeight
        {
            get => PreserveScreenRatio ? _virtualWidth / ScreenRatio : _virtualHeight;
            set => _virtualHeight = value;
        }

        float _viewPortWidth;
        float _viewPortHeight;

        float _virtualWidth = 19;
        float _virtualHeight = 10;

        GameWindow _window;
    }
}
