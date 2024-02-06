﻿using CruZ.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;

namespace CruZ.Systems
{
    public partial class Input : IInputInfo
    {
        public static readonly float MOUSE_CLICK_DURATION = 0.4f;

        public Input(IInputContextProvider contextProvider)
        {
            contextProvider.InputUpdate += InputUpdate;
        }

        public int ScrollDelta()
        {
            return _curMouse.ScrollWheelValue - _preMouse.ScrollWheelValue;
        }

        public DRAW.Point MouseMoveDelta()
        {
            var d = _curMouse.Position - _preMouse.Position;
            return new(d.X, d.Y);
        }

        private void InputUpdate(GameTime gameTime)
        {
            if (!GameApplication.IsActive()) return;

            _preMouse = _curMouse;
            _curMouse = XNA.Input.Mouse.GetState();
            _keyboard = XNA.Input.Keyboard.GetState();

            // Can call IsMouseJustUp/Down after this line

            _scrollDelta = ScrollDelta();
            _mouseScrolling = ScrollDelta() != 0;

            _mouseMoving = DoesMouseMove();

            _mouseStateChanges = DoesMouseStateChange();

            if (IsMouseJustDown(MouseKey.Left))
                _timeSceneLastDownClick = gameTime.TotalSeconds();

            _mouseClick =
                gameTime.TotalSeconds() - _timeSceneLastDownClick < MOUSE_CLICK_DURATION &&
                IsMouseJustUp(MouseKey.Left);

            if (_mouseMoving) MouseMoved?.Invoke(this);
            if (_mouseScrolling) MouseScrolled?.Invoke(this);
            if (_mouseStateChanges) MouseStateChanged?.Invoke(this);
        }

        private bool DoesMouseStateChange()
        {
            return
                GetMouseState(_curMouse, MouseKey.Left) != GetMouseState(_preMouse, MouseKey.Left) ||
                GetMouseState(_curMouse, MouseKey.Middle) != GetMouseState(_preMouse, MouseKey.Middle) ||
                GetMouseState(_curMouse, MouseKey.Right) != GetMouseState(_preMouse, MouseKey.Right);
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

        private MouseState _curMouse;
        private KeyboardState _keyboard;
        private MouseState _preMouse;
        private int _scrollDelta;
        private bool _mouseMoving;
        private float _timeSceneLastDownClick;
        private bool _mouseClick;
        private bool _mouseScrolling;
        private bool _mouseStateChanges;

        public bool MouseClick => _mouseClick;
        public int SrollDelta => _scrollDelta;
        public bool MouseMoving => _mouseMoving;

        public bool MouseStateChanges => _mouseStateChanges;
        
        public KeyboardState Keyboard => _keyboard;
        public MouseState CurMouse => _curMouse;
        public MouseState PreMouse => _preMouse;
        
    }

    public enum MouseKey
    {
        Left,
        Middle,
        Right
    }

    public interface IInputInfo
    {
        int SrollDelta { get; }

        MouseState CurMouse { get; }
        MouseState PreMouse { get; }
        KeyboardState Keyboard { get; }

        bool MouseStateChanges { get; }
        bool MouseClick { get; }
        bool MouseMoving { get; }

        bool IsMouseHeldDown(MouseKey key);
        bool IsMouseHeldUp(MouseKey key);
        bool IsMouseJustDown(MouseKey key);
        bool IsMouseJustUp(MouseKey key);

        DRAW.Point MousePos ()
        {
            return new(CurMouse.X, CurMouse.Y);
        }
    }
}
