using CruZ.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Numerics;

namespace CruZ.Systems
{
    public partial class Input
    {
        public static readonly float MOUSE_CLICK_DURATION = 0.1f;

        public Input(IInputContextProvider contextProvider)
        {
            contextProvider.InputUpdate += InputUpdate;
            _info = new();
        }

        public int ScrollDelta()
        {
            return _info.CurMouse.ScrollWheelValue - _info.PreMouse.ScrollWheelValue;
        }

        public Point MouseMoveDelta()
        {
            return _info.CurMouse.Position - _info.PreMouse.Position;
        }

        private void InputUpdate(GameTime gameTime)
        {
            _info.PreMouse = _info.CurMouse;
            _info.CurMouse = Mouse.GetState();
            _info.Keyboard = Keyboard.GetState();

            // Can call IsMouseUp/Down after this line

            _info.SrollDelta = ScrollDelta();
            _info.DoesMouseScroll = ScrollDelta() != 0;

            _info.DoesMouseMove = DoesMouseMove();

            _info.DoesMouseStateChange = DoesMouseStateChange();

            if(_info.IsMouseDown(MouseKey.Left))
                _info.TimeSceneLastDownClick = gameTime.TotalSeconds();

            _info.DoesMouseClick = 
                gameTime.TotalSeconds() - _info.TimeSceneLastDownClick < MOUSE_CLICK_DURATION &&
                _info.IsMouseUp(MouseKey.Left);
        
            if(_info.DoesMouseMove) MouseMoved?.Invoke(_info);
            if(_info.DoesMouseScroll) MouseScrolled?.Invoke(_info);
            if(_info.DoesMouseStateChange) MouseStateChanged?.Invoke(_info);
            if(_info.DoesMouseClick) MouseClicked?.Invoke(_info);
        }

        private bool DoesMouseStateChange()
        {
            return
                GetMouseState(_info.CurMouse, MouseKey.Left)    != GetMouseState(_info.PreMouse, MouseKey.Left) ||
                GetMouseState(_info.CurMouse, MouseKey.Middle)  != GetMouseState(_info.PreMouse, MouseKey.Middle) ||
                GetMouseState(_info.CurMouse, MouseKey.Right)   != GetMouseState(_info.PreMouse, MouseKey.Right);
        }

        private bool DoesMouseMove()
        {
            return MouseMoveDelta() != Point.Zero;
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
    }

    public enum MouseKey
    {
        Left,
        Middle,
        Right
    }

    public struct InputInfo
    {
        public InputInfo() { }

        public int SrollDelta;

        public MouseState PreMouse;
        public MouseState CurMouse;

        public KeyboardState Keyboard;

        public bool DoesMouseStateChange;
        public bool DoesMouseClick;
        public bool DoesMouseStay;
        public bool DoesMouseMove;
        public bool DoesMouseScroll;

        public float TimeSceneLastDownClick = -1000; // Min number

        public bool IsMouseDown(MouseKey key)
        {
            return
                Input.GetMouseState(CurMouse, key) == ButtonState.Pressed &&
                Input.GetMouseState(PreMouse, key) == ButtonState.Released;
        }

        public bool IsMouseUp(MouseKey key)
        {
            return
                Input.GetMouseState(PreMouse, key) == ButtonState.Pressed &&
                Input.GetMouseState(CurMouse, key) == ButtonState.Released;
        }
    }
}
