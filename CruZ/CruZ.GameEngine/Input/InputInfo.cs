using Microsoft.Xna.Framework.Input;

namespace CruZ.GameEngine.Input
{
    internal struct InputInfo : IInputInfo
    {
        public MouseState CurMouse;
        public MouseState PreMouse;
        public KeyboardState PreKeyboard;
        public KeyboardState CurKeyboard;

        public int ScrollDelta;
        public bool IsMouseMoving;
        public bool IsMouseClick;
        public bool IsMouseScrolling;
        public bool IsMouseStateChange;

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
                CurMouse.ButtonState(key) == ButtonState.Pressed &&
                PreMouse.ButtonState(key) == ButtonState.Released;
        }

        public bool IsMouseJustUp(MouseKey key)
        {
            return
                PreMouse.ButtonState(key) == ButtonState.Pressed &&
                CurMouse.ButtonState(key) == ButtonState.Released;
        }

        public bool IsMouseHeldDown(MouseKey key)
        {
            return
                CurMouse.ButtonState(key) == ButtonState.Pressed;
        }

        public bool IsMouseHeldUp(MouseKey key)
        {
            return
                CurMouse.ButtonState(key) == ButtonState.Released;
        }
    }
}
