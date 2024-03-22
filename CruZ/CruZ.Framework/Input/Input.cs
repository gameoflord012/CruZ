using System;

using CruZ.Common;
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

            _info.preMouse = _preMouseState;
            _preMouseState = Mouse.GetState();
            _info.curMouse = Mouse.GetState();

            _info.preKeyboard = _preKeyboard;
            _preKeyboard = Keyboard.GetState();
            _info.curKeyboard = Keyboard.GetState();
            #endregion

            // Can call IsMouseJustUp/Down after this line

            #region Update Input Actions
            _info.scrollDelta = ScrollDelta();
            _info.mouseScrolling = ScrollDelta() != 0;
            _info.mouseMoving = DoesMouseMove();
            _info.mouseStateChanges = DoesMouseStateChange();

            if (_info.IsMouseJustDown(MouseKey.Left))
                _timeSceneLastDownClick = gameTime.TotalSeconds();

            _info.mouseClick =
                gameTime.TotalSeconds() - _timeSceneLastDownClick < MOUSE_CLICK_DURATION &&
                _info.IsMouseJustUp(MouseKey.Left);
            #endregion

            InvokeEvents();
        }

        public int ScrollDelta()
        {
            return _info.curMouse.ScrollWheelValue - _info.preMouse.ScrollWheelValue;
        }

        public DRAW.Point MouseMoveDelta()
        {
            var d = _info.curMouse.Position - _info.preMouse.Position;
            return new(d.X, d.Y);
        }

        private void InvokeEvents()
        {
            if (_info.mouseMoving) MouseMoved?.Invoke(_info);
            if (_info.mouseScrolling) MouseScrolled?.Invoke(_info);
            if (_info.mouseStateChanges) MouseStateChanged?.Invoke(_info);

            if (_info.preKeyboard.GetHashCode() != _info.curKeyboard.GetHashCode())
                KeyStateChanged?.Invoke(_info);
        }

        private bool DoesMouseStateChange()
        {
            return
                GetMouseState(_info.curMouse, MouseKey.Left) != GetMouseState(_info.preMouse, MouseKey.Left) ||
                GetMouseState(_info.curMouse, MouseKey.Middle) != GetMouseState(_info.preMouse, MouseKey.Middle) ||
                GetMouseState(_info.curMouse, MouseKey.Right) != GetMouseState(_info.preMouse, MouseKey.Right);
        }

        private bool DoesMouseMove()
        {
            return MouseMoveDelta() != new DRAW.Point(0, 0);
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
                    throw new System.Exception();
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
        internal MouseState curMouse;
        internal MouseState preMouse;

        internal KeyboardState preKeyboard;
        internal KeyboardState curKeyboard;

        internal int scrollDelta;
        internal bool mouseMoving;
        internal bool mouseClick;
        internal bool mouseScrolling;
        internal bool mouseStateChanges;
        internal bool isActive;

        public bool MouseClick => mouseClick;
        public int SrollDelta => scrollDelta;
        public bool MouseMoving => mouseMoving;

        public bool MouseStateChanges => mouseStateChanges;

        public KeyboardState Keyboard => curKeyboard;
        public KeyboardState PreKeyboard => preKeyboard;
        public MouseState CurMouse => curMouse;
        public MouseState PreMouse => preMouse;

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

        DRAW.Point MousePos()
        {
            return new(CurMouse.X, CurMouse.Y);
        }
    }
}
