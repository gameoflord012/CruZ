using System;

using CruZ.Common;
using CruZ.Framework.Service;
using CruZ.Framework.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CruZ.Framework.Input
{
    interface IInputController
    {
        void Update(GameTime gameTime);
    }

    public partial class InputManager : IInputController
    {
        private InputManager() { }

        void IInputController.Update(GameTime gameTime)
        {
            _info = new();

            #region Update Input State
            if (!GameApplication.IsActive())
            {
                _isActive = false;
                return;
            }
            else if (!_isActive) // One tick before reactive
            {
                _isActive = true;
                _preMouseState = Mouse.GetState();
                _preKeyboard = Keyboard.GetState();
                _timeSceneLastDownClick = gameTime.TotalSeconds();
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
            _info.IsMouseStateChange = DoesMouseStateChange();

            if (_info.IsMouseJustDown(MouseKey.Left))
                _timeSceneLastDownClick = gameTime.TotalSeconds();

            _info.IsMouseClick =
                gameTime.TotalSeconds() - _timeSceneLastDownClick < MOUSE_CLICK_DURATION &&
                _info.IsMouseJustUp(MouseKey.Left);
            #endregion

            InvokeEvents();

            //
            // update log
            //
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
            if (_info.IsMouseMoving) MouseMoved?.Invoke(_info);
            if (_info.IsMouseScrolling) MouseScrolled?.Invoke(_info);
            if (_info.IsMouseStateChange) MouseStateChanged?.Invoke(_info);

            if (_info.PreKeyboard.GetHashCode() != _info.CurKeyboard.GetHashCode())
                KeyStateChanged?.Invoke(_info);
        }

        private bool DoesMouseStateChange()
        {
            return
                GetMouseState(_info.CurMouse, MouseKey.Left) != GetMouseState(_info.PreMouse, MouseKey.Left) ||
                GetMouseState(_info.CurMouse, MouseKey.Middle) != GetMouseState(_info.PreMouse, MouseKey.Middle) ||
                GetMouseState(_info.CurMouse, MouseKey.Right) != GetMouseState(_info.PreMouse, MouseKey.Right);
        }

        private bool DoesMouseMove()
        {
            return MouseMoveDelta() != new Point(0, 0);
        }

        internal static ButtonState GetMouseState(MouseState state, MouseKey key)
        {
            switch (key)
            {
                case MouseKey.Left:
                    return state.LeftButton;
                case MouseKey.Middle:
                    return state.MiddleButton;
                case MouseKey.Right:
                    return state.RightButton;
                default:
                    throw new Exception();
            }
        }

        InputInfo _info;
        bool _isActive;

        KeyboardState _preKeyboard;
        MouseState _preMouseState;

        float _timeSceneLastDownClick;
        const float MOUSE_CLICK_DURATION = 0.4f;

        public static event Action<IInputInfo>? MouseScrolled;
        public static event Action<IInputInfo>? MouseMoved;
        public static event Action<IInputInfo>? MouseStateChanged;
        public static event Action<IInputInfo>? KeyStateChanged;

        public static IInputInfo Info => _instance._info;

        static InputManager? _instance;

        internal static IInputController CreateContext()
        {
            return _instance = new();
        }
    }

    public enum MouseKey
    {
        Left,
        Middle,
        Right
    }

    struct InputInfo : IInputInfo
    {
        internal MouseState CurMouse;
        internal MouseState PreMouse;

        internal KeyboardState PreKeyboard;
        internal KeyboardState CurKeyboard;

        internal int ScrollDelta;
        internal bool IsMouseMoving;
        internal bool IsMouseClick;
        internal bool IsMouseScrolling;
        internal bool IsMouseStateChange;

        bool IInputInfo.MouseClick => IsMouseClick;
        int IInputInfo.SrollDelta => ScrollDelta;
        bool IInputInfo.MouseMoving => IsMouseMoving;

        public bool MouseStateChanges => IsMouseStateChange;

        KeyboardState IInputInfo.Keyboard => CurKeyboard;
        KeyboardState IInputInfo.PreKeyboard => PreKeyboard;
        MouseState IInputInfo.CurMouse => CurMouse;
        MouseState IInputInfo.PreMouse => PreMouse;

        public bool IsMouseJustDown(MouseKey key)
        {
            return
                InputManager.GetMouseState(CurMouse, key) == ButtonState.Pressed &&
                InputManager.GetMouseState(PreMouse, key) == ButtonState.Released;
        }

        public bool IsMouseJustUp(MouseKey key)
        {
            return
                InputManager.GetMouseState(PreMouse, key) == ButtonState.Pressed &&
                InputManager.GetMouseState(CurMouse, key) == ButtonState.Released;
        }

        public bool IsMouseHeldDown(MouseKey key)
        {
            return
                InputManager.GetMouseState(CurMouse, key) == ButtonState.Pressed;
        }

        public bool IsMouseHeldUp(MouseKey key)
        {
            return
                InputManager.GetMouseState(CurMouse, key) == ButtonState.Released;
        }
    }

    public interface IInputInfo
    {
        int SrollDelta { get; }

        MouseState CurMouse { get; }
        MouseState PreMouse { get; }

        KeyboardState PreKeyboard { get; }
        KeyboardState Keyboard { get; }

        bool MouseStateChanges { get; }
        bool MouseClick { get; }
        bool MouseMoving { get; }

        bool IsMouseHeldDown(MouseKey key);
        bool IsMouseHeldUp(MouseKey key);
        bool IsMouseJustDown(MouseKey key);
        bool IsMouseJustUp(MouseKey key);

        bool IsKeyJustDown(Keys key)
        {
            return PreKeyboard.IsKeyUp(key) && Keyboard.IsKeyDown(key);
        }

        Point MousePos()
        {
            return new(CurMouse.X, CurMouse.Y);
        }
    }
}
