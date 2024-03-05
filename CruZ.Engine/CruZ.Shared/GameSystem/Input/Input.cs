using CruZ.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;

namespace CruZ.GameSystem
{
    public partial class Input
    {
        public static readonly float MOUSE_CLICK_DURATION = 0.4f;

        public Input(IInputContextProvider contextProvider)
        {
            contextProvider.InputUpdate += InputUpdate;
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

        private void InputUpdate(GameTime gameTime)
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
                    throw new global::System.Exception();
            }
        }

        InputInfo _info;
        bool _isActive;

        KeyboardState _preKeyboard;
        MouseState _preMouseState;


        float _timeSceneLastDownClick;
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

        internal int    scrollDelta;
        internal bool   mouseMoving;
        internal bool   mouseClick;
        internal bool   mouseScrolling;
        internal bool   mouseStateChanges;
        internal bool   isActive;

        public bool MouseClick => mouseClick;
        public int  SrollDelta => scrollDelta;
        public bool MouseMoving => mouseMoving;

        public bool MouseStateChanges => mouseStateChanges;

        public KeyboardState    Keyboard => curKeyboard;
        public KeyboardState    PreKeyboard => preKeyboard;
        public MouseState       CurMouse => curMouse;
        public MouseState       PreMouse => preMouse;

        public bool IsMouseJustDown(MouseKey key)
        {
            return
                Input.GetMouseState(CurMouse, key) == ButtonState.Pressed &&
                Input.GetMouseState(PreMouse, key) == ButtonState.Released;
        }

        public bool IsMouseJustUp(MouseKey key)
        {
            return
                Input.GetMouseState(PreMouse, key) == ButtonState.Pressed &&
                Input.GetMouseState(CurMouse, key) == ButtonState.Released;
        }

        public bool IsMouseHeldDown(MouseKey key)
        {
            return
                Input.GetMouseState(CurMouse, key) == ButtonState.Pressed;
        }

        public bool IsMouseHeldUp(MouseKey key)
        {
            return
                Input.GetMouseState(CurMouse, key) == ButtonState.Released;
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
