namespace CruZ.Systems
{
    public partial class Camera
    {
        private static Camera? _mainCamera;

        public static Camera GetMain()
        {
            if (_mainCamera == null)
            {
                _mainCamera = new(ApplicationContext.GraphicsDevice.Viewport);
            }
            return _mainCamera;
        }

        public static Camera? Main
        {
            get => GetMain();
            set => _mainCamera = value;
        }
    }
}