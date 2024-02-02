using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Numerics;

namespace CruZ.Systems
{
    public partial class Input
    {
        public Input(IInputContextProvider contextProvider)
        {
            contextProvider.InputUpdate += InputUpdate;
        }

        public void InputUpdate(GameTime gameTime)
        {
            _prevMouse =   _curMouse;
            _curMouse =    Mouse.GetState();
            _keyboardState =    Keyboard.GetState();

            if(ScrollDelta() != 0)
            {
                MouseScroll?.Invoke(GetInputInfo());
            }

            if(MouseMoveDelta() != Point.Zero)
            {
                MouseMove?.Invoke(GetInputInfo());
            }

            if(
                _curMouse.LeftButton    == ButtonState.Pressed && _prevMouse.LeftButton     != ButtonState.Pressed ||
                _curMouse.RightButton   == ButtonState.Pressed && _prevMouse.RightButton    != ButtonState.Pressed||
                _curMouse.MiddleButton  == ButtonState.Pressed && _prevMouse.MiddleButton   != ButtonState.Pressed)
            {
                MouseDown?.Invoke(GetInputInfo());
            }

            if(
                _curMouse.LeftButton    == ButtonState.Released && _prevMouse.LeftButton     != ButtonState.Released ||
                _curMouse.RightButton   == ButtonState.Released && _prevMouse.RightButton    != ButtonState.Released||
                _curMouse.MiddleButton  == ButtonState.Released && _prevMouse.MiddleButton   != ButtonState.Released)
            {
                MouseUp?.Invoke(GetInputInfo());
            }
        }

        public int ScrollDelta()
        {
            return _curMouse.ScrollWheelValue - _prevMouse.ScrollWheelValue;
        }

        public Point MouseMoveDelta()
        {
            return _curMouse.Position - _prevMouse.Position;
        }

        public InputInfo GetInputInfo()
        {
            InputInfo info = new();
            info.SrollDelta = ScrollDelta();
            info.PrevMouse = _prevMouse;
            info.CurMouse = _curMouse;
            info.Keyboard = _keyboardState;
            return info;
        }

        MouseState      _prevMouse;
        MouseState      _curMouse;
        KeyboardState   _keyboardState;
    }

    public enum MouseKey
    {
        Left,
        Middle,
        Right
    }

    public struct InputInfo
    {
        public int              SrollDelta;
        public MouseState       PrevMouse;
        public MouseState       CurMouse;
        public KeyboardState    Keyboard;

        public bool IsMouseDown(MouseKey key)
        {
            return 
                GetMouseState(CurMouse,  key) == ButtonState.Pressed &&
                GetMouseState(PrevMouse, key) == ButtonState.Released;
        }

        public bool IsAnyMouseDown()
        {
            return
                IsMouseDown(MouseKey.Left) ||
                IsMouseDown(MouseKey.Middle) ||
                IsMouseDown(MouseKey.Right);
        }

        private ButtonState GetMouseState(MouseState state, MouseKey key)
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
    }
}
