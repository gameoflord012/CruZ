using System;

namespace CruZ.Common
{
    public partial class Camera
    {
        private static Camera? _mainCamera;

        private static Camera GetMain()
        {
            if(_mainCamera == null)
            {
                throw new NullReferenceException("Main camera is not assigned");
            }

            return _mainCamera;
        }

        public static Camera Main
        {
            get => GetMain();
            set => _mainCamera = value;
        }
    }
}