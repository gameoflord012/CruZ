using System;

namespace CruZ.Framework
{
    public partial class Camera
    {
        private static Camera? _mainCamera;

        public static Camera Main
        {
            get => _mainCamera ?? throw new InvalidOperationException("Main camera is not assigned");
            set => _mainCamera = value;
        }
    }
}