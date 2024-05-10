using System;

using CruZ.GameEngine.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CruZ.GameEngine.Input
{
    /// <summary>
    /// Frame base input
    /// </summary>
    internal class GameInput
    {
        internal static event Action<IInputInfo>? MouseScrolled;
        internal static event Action<IInputInfo>? MouseMoved;
        internal static event Action<IInputInfo>? MouseStateChanged;
        internal static event Action<IInputInfo>? KeyStateChanged;

        private const float MouseClickDuration = 0.4f;

        private GameInput()
        {
        }

        public void Update(GameTime gameTime)
        {
            _info = new();

            #region Update Input State
            if(!GameApplication.IsActive())
            {
                _isActive = false;
                return;
            }
            else if(!_isActive) // One tick before reactive
            {
                _isActive = true;
                _preMouseState = Mouse.GetState();
                _preKeyboard = Keyboard.GetState();
                _timeSceneLastDownClick = gameTime.TotalGameTime();
            }

            _info.PreMouse = _preMouseState;
            _preMouseState = Mouse.GetState();
            _info.CurMouse = Mouse.GetState();

            _info.PreKeyboard = _preKeyboard;
            _preKeyboard = Keyboard.GetState();
            _info.CurKeyboard = Keyboard.GetState();
            #endregion

            // Can call IsMouseJustUp/Down after this line

            #region Update Input Actions
            _info.ScrollDelta = ScrollDelta();
            _info.IsMouseScrolling = ScrollDelta() != 0;
            _info.IsMouseMoving = DoesMouseMove();
            _info.IsMouseStateChange = DoesMouseButtonChange();

            if(_info.IsMouseJustDown(MouseKey.Left))
            {
                _timeSceneLastDownClick = gameTime.TotalGameTime();
            }

            _info.IsMouseClick =
                gameTime.TotalGameTime() - _timeSceneLastDownClick < MouseClickDuration &&
                _info.IsMouseJustUp(MouseKey.Left);
            #endregion

            InvokeEvents();

            LogManager.SetMsg(_info.CurMouse.Position.ToString(), "CursorCoord");
        }

        public int ScrollDelta()
        {
            return _info.CurMouse.ScrollWheelValue - _info.PreMouse.ScrollWheelValue;
        }

        public Point MouseMoveDelta()
        {
            var d = _info.CurMouse.Position - _info.PreMouse.Position;
            return new(d.X, d.Y);
        }

        private void InvokeEvents()
        {
            if(_info.IsMouseMoving) MouseMoved?.Invoke(_info);
            if(_info.IsMouseScrolling) MouseScrolled?.Invoke(_info);
            if(_info.IsMouseStateChange) MouseStateChanged?.Invoke(_info);

            if(_info.PreKeyboard.GetHashCode() != _info.CurKeyboard.GetHashCode())
            {
                KeyStateChanged?.Invoke(_info);
            }
        }

        private bool DoesMouseButtonChange()
        {
            return
                _info.CurMouse.ButtonState(MouseKey.Left) != _info.PreMouse.ButtonState(MouseKey.Left) ||
                _info.CurMouse.ButtonState(MouseKey.Middle) != _info.PreMouse.ButtonState(MouseKey.Middle) ||
                _info.CurMouse.ButtonState(MouseKey.Right) != _info.PreMouse.ButtonState(MouseKey.Right);
        }

        private bool DoesMouseMove()
        {
            return MouseMoveDelta() != new Point(0, 0);
        }

        private InputInfo _info;
        private bool _isActive;
        private KeyboardState _preKeyboard;
        private MouseState _preMouseState;
        private float _timeSceneLastDownClick;

        internal static GameInput CreateContext()
        {
            if(_instance != null) throw new InvalidOperationException();
            return _instance = new();
        }

        public static IInputInfo GetLastInfo()
        {
            if(_instance == null) throw new InvalidOperationException();
            return _instance._info;
        }

        private static GameInput? _instance;
    }
}
